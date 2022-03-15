using System;
using System.Collections.Generic;
using System.Linq;
using Faster.Ioc.Zero.SourceGenerator.CodeGeneration;
using Faster.Ioc.Zero.SourceGenerator.CodeGeneration.Enums;
using Faster.Map;
using Microsoft.CodeAnalysis;

namespace Faster.Ioc.Zero.SourceGenerator
{
    /// <summary>
    /// 
    /// </summary>
    public class Builder
    {
        #region Fields

        private readonly FastMap<int, IList<BuilderEntry>> _entries = new(256);
        private readonly GeneratorExecutionContext context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Builder" /> class.
        /// </summary>
        public Builder(GeneratorExecutionContext context)
        {
            this.context = context;
        }

        #endregion

        #region Registration Methods   


        /// <summary>
        /// Registers the specified interface.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="paramNames">The parameter names.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="key">The key.</param>
        public void Register(IList<INamedTypeSymbol> arguments, List<string> paramNames, Lifetime lifetime, string key)
        {
            INamedTypeSymbol first;
            INamedTypeSymbol second;

            if (arguments.Count == 2)
            {
                first = arguments[0];
                second = arguments[1];
            }
            else
            {
                first = arguments[0];
                second = arguments[0];
            }

            var entry = new BuilderEntry(first, second, lifetime);
            entry.Hashcode = GenerateHashcode(entry.RegisteredType, key);
            entry.Parameters = paramNames;
            Emplace(entry);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Adds the dependency.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <exception cref="ArgumentNullException">entry</exception>
        private void Emplace(BuilderEntry entry)
        {
            if (!_entries.ContainsKey(entry.Hashcode))
            {
                _entries.Emplace(entry.Hashcode, new List<BuilderEntry> { entry });
                return;
            }

            if (_entries.Get(entry.Hashcode, out var result))
            {
                result.Add(entry);
            }
        }

        /// <summary>
        /// Generates the hashcode.
        /// </summary>
        /// <param name="type">The interface.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private int GenerateHashcode(INamedTypeSymbol type, string key)
        {
            int hashcode;
            if (!string.IsNullOrWhiteSpace(key))
            {
                hashcode = key.GetHashCode();
            }
            else
            {
                hashcode = type.GetHashCode();
            }

            return hashcode;
        }

        public IEnumerable<FileModel> Build(string ns)
        {
            var usings = new List<string> { "System;", "System.Collections.Generic;", "System.Linq;" };

            var disposeableFields = new List<Field>();

            foreach (var registrations in _entries.Values)
            {
                FileModel file = new FileModel { Namespace = ns };
                ClassModel model = new ClassModel("Container");

                model.KeyWords.Add(KeyWord.Static);
                model.KeyWords.Add(KeyWord.Partial);

                var privateFields = new List<Field>();
                var singletonFields = new List<Field>();
                var transientFields = new List<Field>();

                for (var index = 0; index < registrations.Count; index++)
                {
                    var item = registrations[index];

                    model.FileName = item.RegisteredType.Name.Replace("I", "");
                    var @namespace = item.RegisteredType.ContainingNamespace.ToString();

                    if (!usings.Contains($"{@namespace};"))
                    {
                        usings.Add($"{@namespace};");
                    }

                    @namespace = item.ReturnType.ContainingNamespace.ToString();
                    if (!usings.Contains($"{@namespace};"))
                    {
                        usings.Add($"{@namespace};");
                    }

                    if (item.Lifetime == Lifetime.Singleton)
                    {
                        var lazySingletonField = new Field($"Lazy<{item.RegisteredType.Name}>", $"_{ item.ReturnType.Name }".ToLower())
                        {
                            AccessModifier = AccessModifier.Private,
                            KeyWords = new List<KeyWord> { KeyWord.Static },
                            DefaultValue = $"new Lazy<{item.RegisteredType.Name}>(() => new {item.ReturnType.Name}({GenerateParameters(item)}))",
                            Lifetime = Lifetime.Singleton
                        };

                        privateFields.Add(lazySingletonField);

                        var singletonField = new Field(item.RegisteredType.Name, item.ReturnType.Name)
                        {
                            Comment = $"Returns singleton instance of type {item.RegisteredType.Name}",
                            AccessModifier = AccessModifier.Public,
                            KeyWords = new List<KeyWord> { KeyWord.Static },
                            DefaultValue = $"_{ item.ReturnType.Name.ToLower() }.Value",
                            Lifetime = Lifetime.Transient
                        };

                        singletonFields.Add(singletonField);
                        continue;
                    }

                    var transientField = new Field(item.RegisteredType.Name, item.ReturnType.Name)
                    {
                        Comment = $"Returns transient instance of type {item.RegisteredType.Name}",
                        AccessModifier = AccessModifier.Public,
                        KeyWords = new List<KeyWord> { KeyWord.Static },
                        DefaultValue = $"new {item.ReturnType.Name}({GenerateParameters(item)})",
                        Lifetime = item.Lifetime
                    };

                    transientFields.Add(transientField);
                }

                privateFields.AddRange(singletonFields);
                privateFields.AddRange(transientFields);

                disposeableFields.AddRange(singletonFields);

                var getAll = GenerateGetAll(privateFields);
                if (getAll != null)
                {
                    model.Methods.Add(getAll);
                }

                model.Fields = privateFields.ToList();

                file.LoadUsingDirectives(usings);
                file.Classes.Add(model);

                yield return file;
            }

            FileModel disposableFile = new FileModel { Namespace = ns };
            ClassModel disposeClassModel = new ClassModel("Container");

            disposeClassModel.KeyWords.Add(KeyWord.Static);
            disposeClassModel.KeyWords.Add(KeyWord.Partial);

            var dispose = new Method(AccessModifier.Public, KeyWord.Static, BuiltInDataType.Void, "Dispose")
            {
                BodyLines = disposeableFields.Select(i => $"TryDispose({i.Name}); {Environment.NewLine}").ToList()
            };

            disposeClassModel.FileName = "Disposeable";
            disposeClassModel.Methods.Add(dispose);
            disposableFile.Classes.Add(disposeClassModel);
            yield return disposableFile;
        }

        /// <summary>
        /// Generates Ienumerable<T> returning all registered entries
        /// </summary>
        /// <param name="fields">The fields.</param>
        private Method GenerateGetAll(List<Field> fields)
        {
            var implementations = new List<string>();
            Field field = default;
            foreach (var item in fields)
            {
                if (!item.Name.StartsWith("_"))
                {
                    field = item;
                    implementations.Add($"yield return {item.Name};");
                }
            }

            var enumerable = new Method($"IEnumerable<{field?.CustomDataType}>", $"GetAll{field?.CustomDataType}")
            {
                Comment = $"Returns all instances of type {field?.CustomDataType}",
                AccessModifier = AccessModifier.Public,
                KeyWords = new List<KeyWord> { KeyWord.Static },
                BodyLines = implementations
            };

            return enumerable;
        }

        /// <summary>
        /// Generates the parameters.
        /// </summary>
        /// <returns></returns>
        private string GenerateParameters(BuilderEntry entry)
        {
            //override constructor
            if (entry.Parameters != null & entry.Parameters.Any())
            {
                //add , to all parameters
                return string.Join(", ", entry.Parameters);
            }

            //get constructor with largest param count
            var constructor =
                entry.ReturnType.Constructors.OrderByDescending(i => i.Parameters.Length).FirstOrDefault() ??
                entry.ReturnType.Constructors.FirstOrDefault();

            if (constructor == null)
            {
                var error = Diagnostic.Create(new DiagnosticDescriptor("RS0004", "Constructor", $"Please specify a constructor {entry.ReturnType.Name}", "Registration Error", DiagnosticSeverity.Error, true), null,
                    DiagnosticSeverity.Error);
                context.ReportDiagnostic(error);
            }

            var parameters = constructor.Parameters.ToList();
            if (parameters.Count == 0)
            {
                return string.Empty;
            }

            var resolved = new List<string>();

            foreach (var parameter in parameters)
            {
                //Resolve arrays 
                if (parameter.Type is IArrayTypeSymbol arrayTypeSymbol)
                {
                    var isArray = arrayTypeSymbol.OriginalDefinition.ToString().IndexOf("[]", StringComparison.Ordinal) != -1;
                    if (isArray)
                    {
                        var name = ((INamedTypeSymbol)((IArrayTypeSymbol)arrayTypeSymbol.OriginalDefinition).ElementType).OriginalDefinition.Name;
                        resolved.Add($"GetAll{name}().ToArray()");
                    }
                    continue;
                }

                //Resolve Ienumerables
                var type = (INamedTypeSymbol)parameter.Type;
                var isIEnumerable = type.OriginalDefinition.ToString().IndexOf("IEnumerable<T>", StringComparison.Ordinal) != -1;
                if (isIEnumerable)
                {
                    var argumentes = type.TypeArguments;
                    if (argumentes.Length > 1)
                    {
                        var error = Diagnostic.Create(new DiagnosticDescriptor("RS0003", "Generic arguments", $"Multiple generic arguments ar enot supported {parameter.Type}", "Registration Error", DiagnosticSeverity.Error, true), type.Locations[0],
                            DiagnosticSeverity.Error);
                        context.ReportDiagnostic(error);
                    }

                    resolved.Add($"GetAll{type.TypeArguments[0].Name}()");
                    continue;
                }

                //Resolve collections
                var isCollection = type.OriginalDefinition.ToString().IndexOf("System.Collections", StringComparison.Ordinal) != -1;
                if (isCollection)
                {
                    var argumentes = type.TypeArguments;
                    if (argumentes.Length > 1)
                    {
                        var error = Diagnostic.Create(new DiagnosticDescriptor("RS0002", "Generic arguments", $"Multiple generic arguments ar enot supported {parameter.Type}", "Registration Error", DiagnosticSeverity.Error, true), type.Locations[0],
                            DiagnosticSeverity.Error);
                        context.ReportDiagnostic(error);
                    }

                    resolved.Add($"GetAll{type.TypeArguments[0].Name}().ToList()");
                    continue;
                }


                if (_entries.Get(GenerateHashcode((INamedTypeSymbol)parameter.Type, string.Empty), out var result))
                {
                    var p = result.FirstOrDefault();
                    resolved.Add(p.ReturnType.Name);
                }
                else
                {
                    var error = Diagnostic.Create(new DiagnosticDescriptor("RS0001", "No registration found", $"No registration found of type {parameter.Type}", "Registration Error", DiagnosticSeverity.Error, true), type.Locations[0],
                        DiagnosticSeverity.Error);
                    context.ReportDiagnostic(error);
                }
            }

            return string.Join(", ", resolved);
        }

        #endregion
    }
}
