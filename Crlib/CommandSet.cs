﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace REVUnit.Crlib
{
    public class CommandSet<TReturn, TParam> where TReturn : class
    {
        public delegate TReturn CommandMethod(TParam[] parameters);

        private const BindingFlags TargetFlags = BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public;

        private readonly List<CommandMethod> commands = new List<CommandMethod>();

        public void AddCommand(IEnumerable<CommandMethod> commandDelegates)
        {
            commands.AddRange(commandDelegates);
        }

        public void AddCommand(IEnumerable<MethodInfo> methodInfos)
        {
            if (methodInfos == null) throw new ArgumentNullException(nameof(methodInfos));
            commands.AddRange(methodInfos.Select(m => (CommandMethod) m.CreateDelegate(typeof(CommandMethod))));
        }

        public void AddCommand(Type container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            commands.AddRange(
                container.GetMethods(TargetFlags)
                    .Select(m => (CommandMethod) m.CreateDelegate(typeof(CommandMethod))));
        }

        public void AddCommand<TMethodAttribute>(Type container) where TMethodAttribute : Attribute
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            commands.AddRange(
                container.GetMethods(TargetFlags)
                    .Where(m => m.GetCustomAttributes<TMethodAttribute>().Any())
                    .Select(m => (CommandMethod) m.CreateDelegate(typeof(CommandMethod))));
        }

        public void ClearCommands()
        {
            commands.Clear();
        }

        public TReturn RunCommand(string name, TParam[] parameters = null, bool throwOnNotFound = false)
        {
            parameters ??= Array.Empty<TParam>();
            CommandMethod commandMethod = commands.Find(c => c.Method.Name == name);
            if (commandMethod == null)
            {
                if (throwOnNotFound) throw new NullReferenceException("Command not found.");
                return default;
            }

            return commandMethod(parameters);
        }
    }
}