﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WarpScheduling.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WarpScheduling.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a SELECT p.Warp_MO, p.Expr1, Sum(p.Tickets) TotalRolls, Min(p.DueDate) FROM manufacturing.`plan log` p
        ///Where p.WarpProcessed =0 and p.duedate is not null and p.duedate Not Like &apos;0001-01-01%&apos;
        ///Group by p.Warp_MO, p.Expr1 
        ///Order by Warp_MO LIMIT 200 ;.
        /// </summary>
        internal static string NewWarps {
            get {
                return ResourceManager.GetString("NewWarps", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Select a.ItemNumber, a.ComponentItemNumber, ii.ItemDescription, RequiredQuantity From  (Select i.ItemNumber, i.ItemDescription, i.ItemKey, b.ComponentItemNumber , b.ComponentItemKey, b.RequiredQuantity
        ///From dbo.FS_Item i  Inner Join dbo.FS_BillOfMaterial b on i.ItemKey=b.ParentItemKey
        ///Where i.ItemNumber Like &apos;WS%&apos; and i.ItemStatus &lt;&gt;&apos;O&apos; ) a Inner Join dbo.FS_Item ii on a.ComponentItemKey=ii.ItemKey
        ///Where RequiredQuantity&gt;.009 
        ///Group by a.ItemNumber, a.ComponentItemNumber, ii.ItemDescription, RequiredQua [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string WBIll {
            get {
                return ResourceManager.GetString("WBIll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Select Warp_MO,Customer, Item, Sum(Tickets) Ticket From
        ///manufacturing.`Plan Log`
        ///Where DueDate&gt; AddDate(CurDate(),Interval -180 Day)
        ///Group by Warp_MO, Customer, Item.
        /// </summary>
        internal static string WpMOCustomer {
            get {
                return ResourceManager.GetString("WpMOCustomer", resourceCulture);
            }
        }
    }
}
