﻿// <auto-generated/>

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#nullable enable

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Framework;
using DocumentFormat.OpenXml.Framework.Metadata;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation.Schema;
using System;
using System.Collections.Generic;
using System.IO.Packaging;

namespace DocumentFormat.OpenXml.Office.SpreadSheetML.Y2022.PivotRichData
{
    /// <summary>
    /// <para>Defines the RichDataPivotCacheGuid Class.</para>
    /// <para>This class is available in Microsoft365 and above.</para>
    /// <para>When the object is serialized out as xml, it's qualified name is xprd:richInfo.</para>
    /// </summary>
    public partial class RichDataPivotCacheGuid : TypedOpenXmlLeafElement
    {
        /// <summary>
        /// Initializes a new instance of the RichDataPivotCacheGuid class.
        /// </summary>
        public RichDataPivotCacheGuid() : base()
        {
        }

        /// <summary>
        /// <para>pivotCacheGuid, this property is only available in Microsoft365 and later.</para>
        /// <para>Represents the following attribute in the schema: pivotCacheGuid</para>
        /// </summary>
        public StringValue? PivotCacheGuid
        {
            get => GetAttribute<StringValue>();
            set => SetAttribute(value);
        }

        internal override void ConfigureMetadata(ElementMetadata.Builder builder)
        {
            base.ConfigureMetadata(builder);
            builder.SetSchema("xprd:richInfo");
            builder.Availability = FileFormatVersions.Microsoft365;
            builder.AddElement<RichDataPivotCacheGuid>()
                .AddAttribute("pivotCacheGuid", a => a.PivotCacheGuid, aBuilder =>
                {
                    aBuilder.AddValidator(RequiredValidator.Instance);
                    aBuilder.AddValidator(new StringValidator() { IsToken = (true), Pattern = ("\\{[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12}\\}") });
                });
        }

        /// <inheritdoc/>
        public override OpenXmlElement CloneNode(bool deep) => CloneImp<RichDataPivotCacheGuid>(deep);
    }
}