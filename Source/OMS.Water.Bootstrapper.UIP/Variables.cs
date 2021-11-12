#region

using System.Collections.Generic;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

#endregion


namespace OMS.Water.Bootstrapper
{
    internal sealed class Variables<TVariable> : IVariables<TVariable>
    {
        public Variables( Engine.Variables<TVariable> engineVariables )
        {
            EngineVariables = engineVariables;
        }


        private Engine.Variables<TVariable> EngineVariables { get; set; }


        #region IVariables<TVariable> Members

        public TVariable this[ string name ]
        {
            get
            {
                TVariable variable;
                if( !TryGetValue( name, out variable ) )
                    throw new KeyNotFoundException( string.Format( "Unknown variable '{0}'", name ) );

                return variable;
            }
            set { EngineVariables[name] = value; }
        }


        public bool TryGetValue( string key, out TVariable variable )
        {
            if( !EngineVariables.Contains( key ) )
            {
                variable = default(TVariable);
                return false;
            }

            variable = EngineVariables[key];
            return true;
        }

        #endregion
    }
}
