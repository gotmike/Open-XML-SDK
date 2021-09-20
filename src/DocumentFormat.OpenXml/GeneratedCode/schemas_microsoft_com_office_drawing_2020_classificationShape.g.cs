﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#nullable enable

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Framework;
using DocumentFormat.OpenXml.Framework.Metadata;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation.Schema;
using System;
using System.Collections.Generic;
using System.IO.Packaging;

namespace DocumentFormat.OpenXml.Office2021.Drawing.DocumentClassification
{
    /// <summary>
    /// <para>Defines the ClassificationOutcome Class.</para>
    /// <para>This class is available in Office 2021 and above.</para>
    /// <para>When the object is serialized out as xml, it's qualified name is aclsh:classification.</para>
    /// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
    [SchemaAttr(119, "classification")]
#pragma warning restore CS0618 // Type or member is obsolete
    public partial class ClassificationOutcome : OpenXmlLeafElement
    {
        /// <summary>
        /// Initializes a new instance of the ClassificationOutcome class.
        /// </summary>
        public ClassificationOutcome() : base()
        {
        }

        /// <summary>
        /// <para>classificationOutcomeType, this property is only available in Office 2021 and later.</para>
        /// <para>Represents the following attribute in the schema: classificationOutcomeType</para>
        /// </summary>

#pragma warning disable CS0618 // Type or member is obsolete

        [SchemaAttr(0, "classificationOutcomeType")]
#pragma warning restore CS0618 // Type or member is obsolete

        public EnumValue<DocumentFormat.OpenXml.Office2021.Drawing.DocumentClassification.ClassificationOutcomeType>? ClassificationOutcomeType
        {
            get => GetAttribute<EnumValue<DocumentFormat.OpenXml.Office2021.Drawing.DocumentClassification.ClassificationOutcomeType>>();
            set => SetAttribute(value);
        }

        internal override void ConfigureMetadata(ElementMetadata.Builder builder)
        {
            base.ConfigureMetadata(builder);
            builder.SetSchema(119, "classification");
            builder.Availability = FileFormatVersions.Office2021;
            builder.AddElement<ClassificationOutcome>()
.AddAttribute(0, "classificationOutcomeType", a => a.ClassificationOutcomeType, aBuilder =>
{
aBuilder.AddValidator(new StringValidator() { IsToken = (true) });
});
        }

        /// <inheritdoc/>
        public override OpenXmlElement CloneNode(bool deep) => CloneImp<ClassificationOutcome>(deep);
    }

    /// <summary>
    /// Defines the ClassificationOutcomeType enumeration.
    /// </summary>
    [OfficeAvailability(FileFormatVersions.Office2021)]
    public enum ClassificationOutcomeType
    {
        ///<summary>
        ///none.
        ///<para>When the item is serialized out as xml, its value is "none".</para>
        ///</summary>
        [EnumString("none")]
        None,
        ///<summary>
        ///hdr.
        ///<para>When the item is serialized out as xml, its value is "hdr".</para>
        ///</summary>
        [EnumString("hdr")]
        Hdr,
        ///<summary>
        ///ftr.
        ///<para>When the item is serialized out as xml, its value is "ftr".</para>
        ///</summary>
        [EnumString("ftr")]
        Ftr,
        ///<summary>
        ///watermark.
        ///<para>When the item is serialized out as xml, its value is "watermark".</para>
        ///</summary>
        [EnumString("watermark")]
        Watermark,
    }
}