using OpenGui.Controls;
using OpenGui.Core;
using OpenGui.Values;
using Portable.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenGui.App
{
    /// <summary>
    /// The base class for a component
    /// </summary>
    public class Component : ViewContainer<View>
    {
        private bool _initialized = false;

        public Component() : base(1)
        {
            BindingContext = this;
            
            var type = this.GetType();
            var componentattr = Attribute.GetCustomAttribute(type, typeof(ComponentAttribute)) as ComponentAttribute;
            if(componentattr != null)
            {
                if (!string.IsNullOrEmpty(componentattr.Template))
                    CompileXaml(componentattr.Template);
                else if(!string.IsNullOrEmpty(componentattr.TemplateResource))
                {
                    using (var resourceStream = Assembly.GetAssembly(type).GetManifestResourceStream(componentattr.TemplateResource))
                        using (var reader = new StreamReader(resourceStream))
                        CompileXaml(reader.ReadToEnd());
                }
                else if(!string.IsNullOrEmpty(componentattr.TemplateFile))
                {
                    var xaml = File.ReadAllText(componentattr.TemplateFile);
                    CompileXaml(xaml);
                }
            }

            ViewCreated();

        }

        protected virtual void ViewCreated()
        {

        }

        protected virtual void Initialize()
        {

        }

        public override void Check()
        {
            base.Check();

            if(!_initialized)
            {
                Initialize();
                _initialized = true;
            }
            
        }

        private void CompileXaml(string xaml)
        {
            var content = (View)XamlServices.Parse(xaml);
            Children.Add(content);
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            if (Children.Count > 0)
            {
                var child = Children[0];
                child.Mesure(widthSpec, heightSpec, mode);
                OnLayout();
                return (child.CalculatedWidth, child.CalculatedHeight);
            }

            return base.OnMesure(widthSpec, heightSpec, mode);
        }
        
        protected override void OnLayout()
        {
            if (Children.Count == 0)
                return;

            var child = Children[0];
            child.X = X;
            child.Y = X;
        }
    }
}
