﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FantasyCritic.Lib.Statistics {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FantasyCritic.Lib.Statistics.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to library(dplyr)
        ///#library(tidytext)
        ///library(tidyr)
        ///#library(ggplot2)
        ///
        ///# read CSV file
        ///args &lt;- commandArgs()
        ///inputFilePath &lt;- args[2]
        ///dat &lt;- read.csv(inputFilePath)
        ///
        ///# select meaningful + response vars
        ///ds &lt;- dat %&gt;%
        ///  select(EligiblePercentStandardGame,EligiblePercentCounterPick,DateAdjustedHypeFactor,AverageDraftPosition,TotalBidAmount,BidPercentile,CriticScore)
        ///
        ///# Replacing all zeroes to NA:
        ///ds[ds == 0] &lt;- NA
        ///
        ///# summary(ds)
        ///
        ///# replace NAs with column Means
        ///ds &lt;- replace_na(ds,as.list(colM [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MasterGameStatisticsScript {
            get {
                return ResourceManager.GetString("MasterGameStatisticsScript", resourceCulture);
            }
        }
    }
}