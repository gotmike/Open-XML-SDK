﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Packaging.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;

namespace DocumentFormat.OpenXml.Features;

internal abstract class PackageFeatureBase : IPackage, IPackageFeature, IRelationshipFilterFeature
{
    private static readonly Uri _packageUri = new("/", UriKind.Relative);

    private Action<PackageRelationshipBuilder>? _relationshipFilter;
    private RelationshipCache _relationships;
    private Dictionary<Uri, PackagePart>? _parts;

    protected PackageFeatureBase()
    {
        _relationships = new(this);
    }

    protected abstract Package Package { get; }

    private IPackagePart GetOrCreatePart(System.IO.Packaging.PackagePart part)
    {
        if (_parts is not null && _parts.TryGetValue(part.Uri, out var existing))
        {
            Debug.Assert(existing.Part == part);
            return existing;
        }

        if (_parts is null)
        {
            _parts = new();
        }

        var newItem = new PackagePart(this, part);
        _parts[part.Uri] = newItem;
        return newItem;
    }

    protected void UpdateCachedItems()
    {
        if (_parts is null)
        {
            return;
        }

#if !NET6_0_OR_GREATER
        List<Uri>? unused = null;
#endif

        foreach (var part in _parts)
        {
            if (Package.PartExists(part.Key))
            {
                part.Value.Part = Package.GetPart(part.Key);
            }
            else
            {
#if NET6_0_OR_GREATER
                _parts.Remove(part.Key);
#else
                (unused ??= new()).Add(part.Key);
#endif
            }

#if !NET6_0_OR_GREATER
            if (unused is not null)
            {
                foreach (var uri in unused)
                {
                    _parts.Remove(uri);
                }
            }
#endif
        }
    }

    FileAccess IPackage.FileOpenAccess => Package.FileOpenAccess;

    public PackageProperties PackageProperties => Package.PackageProperties;

    IEnumerable<IPackagePart> IPackage.GetParts()
    {
        foreach (var part in Package.GetParts())
        {
            yield return GetOrCreatePart(part);
        }
    }

    IPackagePart IPackage.GetPart(Uri uriTarget) => GetOrCreatePart(Package.GetPart(uriTarget));

    bool IPackage.PartExists(Uri partUri) => Package.PartExists(partUri);

    void IPackage.Save() => Package.Flush();

    public IPackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string? relationshipType, string? id)
        => _relationships.GetOrCreate(_packageUri, Package.CreateRelationship(targetUri, targetMode, relationshipType, id));

    void IPackage.DeleteRelationship(string id)
    {
        _relationships.Remove(id);
        Package.DeleteRelationship(id);
    }

    public IEnumerable<IPackageRelationship> GetRelationships()
    {
        foreach (var relationship in Package.GetRelationships())
        {
            yield return _relationships.GetOrCreate(_packageUri, relationship);
        }
    }

    public IPackagePart CreatePart(Uri partUri, string contentType, CompressionOption compressionOption) => GetOrCreatePart(Package.CreatePart(partUri, contentType, compressionOption));

    public bool RelationshipExists(string relationship) => Package.RelationshipExists(relationship);

    public void DeletePart(Uri uri)
    {
        _parts?.Remove(uri);
        Package.DeletePart(uri);
    }

    public IPackageRelationship GetRelationship(string id)
        => _relationships.GetOrCreate(_packageUri, Package.GetRelationship(id));

    IPackage IPackageFeature.Package => this;

    public virtual PackageCapabilities Capabilities
    {
        get
        {
            // ZipArchive.Flush only exists on .NET Framework (https://github.com/dotnet/runtime/issues/24149)
#if NETFRAMEWORK
            var canSave = Package.FileOpenAccess.HasFlagFast(FileAccess.Write) ? PackageCapabilities.Save : PackageCapabilities.None;
            var uriSupport = GetSupportsMalformedUri() ? PackageCapabilities.MalformedUri : PackageCapabilities.None;

            return canSave | uriSupport | PackageCapabilities.Cached | PackageCapabilities.LargePartStreams;
#else
            return PackageCapabilities.Cached;
#endif
        }
    }

#if NETFRAMEWORK
    private static bool GetSupportsMalformedUri()
        => Uri.TryCreate("mailto:one@", UriKind.RelativeOrAbsolute, out _);
#endif

    public virtual void Reload(FileMode? mode = null, FileAccess? access = null)
        => throw new NotSupportedException();

    internal void RunFilter(PackageRelationshipBuilder builder) => _relationshipFilter?.Invoke(builder);

    void IRelationshipFilterFeature.AddFilter(Action<PackageRelationshipBuilder> action)
        => _relationshipFilter += action;

    private sealed class PackagePart : IPackagePart
    {
        private RelationshipCache _relationships;

        public PackagePart(PackageFeatureBase package, System.IO.Packaging.PackagePart part)
        {
            _relationships = new(package);

            Part = part;
        }

        public System.IO.Packaging.PackagePart Part { get; set; }

        IPackage IPackagePart.Package => _relationships.Feature;

        public Uri Uri => Part.Uri;

        public string ContentType => Part.ContentType;

        public void DeleteRelationship(string id)
        {
            _relationships.Remove(id);
            Part.DeleteRelationship(id);
        }

        public IEnumerable<IPackageRelationship> GetRelationships()
        {
            foreach (var relationship in Part.GetRelationships())
            {
                yield return _relationships.GetOrCreate(Part.Uri, relationship);
            }
        }

        public Stream GetStream(FileMode open, FileAccess write)
            => Part.GetStream(open, write);

        public IPackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType, string? id)
            => _relationships.GetOrCreate(Part.Uri, Part.CreateRelationship(targetUri, targetMode, relationshipType, id));

        public bool RelationshipExists(string relationship)
            => Part.RelationshipExists(relationship);

        public IPackageRelationship GetRelationship(string id)
            => _relationships.GetOrCreate(Part.Uri, Part.GetRelationship(id));
    }

    private struct RelationshipCache
    {
        private Dictionary<string, PackageRelationshipBuilder>? _relationships;

        public RelationshipCache(PackageFeatureBase feature)
        {
            Feature = feature;
        }

        public PackageFeatureBase Feature { get; }

        public IPackageRelationship GetOrCreate(Uri partUri, PackageRelationship relationship)
        {
            if (_relationships is not null && _relationships.TryGetValue(relationship.Id, out var existing))
            {
                return existing;
            }

            _relationships ??= new Dictionary<string, PackageRelationshipBuilder>();

            var newItem = new PackageRelationshipBuilder(partUri, relationship);
            Feature.RunFilter(newItem);
            _relationships[relationship.Id] = newItem;
            return newItem;
        }

        internal void Remove(string id) => _relationships?.Remove(id);
    }
}
