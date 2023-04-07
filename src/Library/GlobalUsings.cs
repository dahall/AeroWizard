#if NETFRAMEWORK
global using CodeDomSerializer = System.ComponentModel.Design.Serialization.CodeDomSerializer;
//global using IToolboxUser = System.Drawing.Design.IToolboxUser;
#else
global using CodeDomSerializer = Microsoft.DotNet.DesignTools.Serialization.CodeDomSerializer;
//global using IToolboxUser = System.Windows.Forms.Design.IToolboxUser;
#endif
