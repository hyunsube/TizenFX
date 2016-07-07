﻿// Copyright 2016 by Samsung Electronics, Inc.,
//
// This software is the confidential and proprietary information
// of Samsung Electronics, Inc. ("Confidential Information"). You
// shall not disclose such Confidential Information and shall use
// it only in accordance with the terms of the license agreement
// you entered into with Samsung.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Tizen.Applications
{
    /// <summary>
    /// This class provide methods and properties to get information about packages.
    /// </summary>
    public class Package
    {
        private const string LogTag = "Tizen.Applications";

        private string _id = string.Empty;
        private string _label = string.Empty;
        private string _iconPath = string.Empty;
        private string _version = string.Empty;
        private PackageType _type;
        private Interop.PackageManager.StorageType _installedStorageType;
        private string _rootPath = string.Empty;
        private string _expansionPackageName = string.Empty;
        private bool _isSystemPackage;
        private bool _isRemovable;
        private bool _isPreloaded;
        private bool _isAccessible;
        private IReadOnlyDictionary<CertificateType, PackageCertificate> _certificates;
        private List<string> _privileges;

        private Package(string pkgId)
        {
            _id = pkgId;
        }

        /// <summary>
        /// Package ID.
        /// </summary>
        public string Id { get { return _id; } }

        /// <summary>
        /// Label of the package.
        /// </summary>
        public string Label { get { return _label; } }

        /// <summary>
        /// Absolute path to the icon image.
        /// </summary>
        public string IconPath { get { return _iconPath; } }

        /// <summary>
        /// Version of the package.
        /// </summary>
        public string Version { get { return _version; } }

        /// <summary>
        /// Type of the package.
        /// </summary>
        public PackageType PackageType { get { return _type; } }

        /// <summary>
        /// Installed storage type for the package.
        /// </summary>
        public StorageType InstalledStorageType { get { return (StorageType)_installedStorageType; } }

        /// <summary>
        /// Root path for the package.
        /// </summary>
        public string RootPath { get { return _rootPath; } }

        /// <summary>
        /// Expansion package name for the package.
        /// </summary>
        public string TizenExpansionPackageName { get { return _expansionPackageName; } }

        /// <summary>
        /// Checks whether the package is system package.
        /// </summary>
        public bool IsSystemPackage { get { return _isSystemPackage; } }

        /// <summary>
        /// Checks whether the package is removable.
        /// </summary>
        public bool IsRemovable { get { return _isRemovable; } }

        /// <summary>
        /// Checks whether the package is preloaded.
        /// </summary>
        public bool IsPreloaded { get { return _isPreloaded; } }

        /// <summary>
        /// Checks whether the current package is accessible.
        /// </summary>
        public bool IsAccessible { get { return _isAccessible; } }

        /// <summary>
        /// Certificate information for the package
        /// </summary>
        public IReadOnlyDictionary<CertificateType, PackageCertificate> Certificates { get { return _certificates; } }

        /// <summary>
        /// Requested privilege for the package
        /// </summary>
        public IEnumerable<string> Privileges { get { return _privileges; } }

        /// <summary>
        /// Clears the application's internal and external cache directory.
        /// </summary>
        /// <exception cref="OutOfMemoryException">Thrown when there is not enough memory to continue the execution of the method</exception>
        /// <exception cref="System.IO.IOException">Thrown when method failed due to internal IO error</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when app does not have privilege to access this method</exception>
        /// <exception cref="SystemException">Thrown when method failed due to internal system error</exception>
        /// <privilege>http://tizen.org/privilege/packagemanager.clearcache</privilege>
        public void ClearCacheDirectory()
        {
            Interop.PackageManager.ErrorCode err = Interop.PackageManager.PackageManagerClearCacheDir(Id);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, string.Format("Failed to clear cache directory for {0}. err = {1}", Id, err));
                throw PackageManagerErrorFactory.GetException(err, "Failed to clear cache directory");
            }
        }

        /// <summary>
        /// Retrieves all application IDs of this package.
        /// </summary>
        /// <returns>Returns a dictionary containing all application info for given application type asynchronously.</returns>
        public IEnumerable<ApplicationInfo> GetApplications()
        {
            return GetApplications(ApplicationType.All);
        }

        /// <summary>
        /// Retrieves all application IDs of this package.
        /// </summary>
        /// <param name="type">Optional: AppType enum value</param>
        /// <returns>Returns a dictionary containing all application info for given application type asynchronously.</returns>
        public IEnumerable<ApplicationInfo> GetApplications(ApplicationType type)
        {
            List<ApplicationInfo> appInfoList = new List<ApplicationInfo>();
            Interop.Package.PackageInfoAppInfoCallback cb = (Interop.Package.AppType appType, string appId, IntPtr userData) =>
            {
                appInfoList.Add(new ApplicationInfo(appId));
                return true;
            };

            IntPtr packageInfoHandle;
            Interop.PackageManager.ErrorCode err = Interop.Package.PackageInfoCreate(Id, out packageInfoHandle);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, string.Format("Failed to create native handle for package info. err = {0}", err));
            }

            err = Interop.Package.PackageInfoForeachAppInfo(packageInfoHandle, (Interop.Package.AppType)type, cb, IntPtr.Zero);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, string.Format("Failed to application info. err = {0}", err));
            }

            err = Interop.Package.PackageInfoDestroy(packageInfoHandle);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, string.Format("Failed to destroy native handle for package info. err = {0}", err));
            }
            return appInfoList;
        }

        /// <summary>
        /// Gets size information for this package.
        /// </summary>
        /// <returns>package size information</returns>
        /// <privilege>http://tizen.org/privilege/packagemanager.info</privilege>
        public async Task<PackageSizeInformation> GetSizeInformationAsync()
        {
            TaskCompletionSource<PackageSizeInformation> tcs = new TaskCompletionSource<PackageSizeInformation>();
            Interop.PackageManager.PackageManagerSizeInfoCallback sizeInfoCb = (pkgId, sizeInfoHandle, userData) =>
            {
                if (sizeInfoHandle != IntPtr.Zero && Id == pkgId)
                {
                    var pkgSizeInfo = PackageSizeInformation.GetPackageSizeInformation(sizeInfoHandle);
                    tcs.TrySetResult(pkgSizeInfo);
                }
            };

            Interop.PackageManager.ErrorCode err = Interop.PackageManager.PackageManagerGetSizeInfo(Id, sizeInfoCb, IntPtr.Zero);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                tcs.TrySetException(PackageManagerErrorFactory.GetException(err, "Failed to get total package size info"));
            }
            return await tcs.Task.ConfigureAwait(false);
        }

        // This method assumes that given arguments are already validated and have valid values.
        internal static Package CreatePackage(IntPtr handle, string pkgId)
        {
            Package package = new Package(pkgId);

            var err = Interop.PackageManager.ErrorCode.None;
            err = Interop.Package.PackageInfoGetLabel(handle, out package._label);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get label for package");
            }
            err = Interop.Package.PackageInfoGetIconPath(handle, out package._iconPath);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get label for icon path");
            }
            err = Interop.Package.PackageInfoGetVersion(handle, out package._version);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get package version");
            }

            string type;
            Interop.Package.PackageInfoGetType(handle, out type);
            if (Enum.TryParse(type, true, out package._type) == false)
            {
                Log.Warn(LogTag, "Failed to get package type");
            }
            err = Interop.Package.PackageInfoGetRootPath(handle, out package._rootPath);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get package root directory");
            }
            err = Interop.Package.PackageInfoGetTepName(handle, out package._expansionPackageName);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get expansion package name");
                package._expansionPackageName = string.Empty;
            }

            err = Interop.Package.PackageInfoGetInstalledStorage(handle, out package._installedStorageType);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get installed storage type for package");
            }
            Interop.Package.PackageInfoIsSystemPackage(handle, out package._isSystemPackage);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get if package is system package");
            }
            Interop.Package.PackageInfoIsRemovablePackage(handle, out package._isRemovable);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get if package is removable");
            }
            Interop.Package.PackageInfoIsPreloadPackage(handle, out package._isPreloaded);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get if package is preloaded");
            }
            Interop.Package.PackageInfoIsAccessible(handle, out package._isAccessible);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, "Failed to get if package is accessible");
            }

            package._certificates = PackageCertificate.GetPackageCertificates(handle);
            package._privileges = GetPackagePrivilegeInformation(handle);
            return package;
        }

        internal static Package GetPackage(string packageId)
        {
            IntPtr packageInfoHandle;
            Interop.PackageManager.ErrorCode err = Interop.Package.PackageInfoCreate(packageId, out packageInfoHandle);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                throw PackageManagerErrorFactory.GetException(err, "Failed to create native handle for package info.");
            }

            Package package = CreatePackage(packageInfoHandle, packageId);

            err = Interop.Package.PackageInfoDestroy(packageInfoHandle);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, string.Format("Failed to destroy native handle for package info. err = {0}", err));
            }
            return package;
        }

        internal static Package GetPackage(IntPtr packageInfoHandle)
        {
            String packageId;
            Interop.PackageManager.ErrorCode err = Interop.Package.PackageInfoGetPackage(packageInfoHandle, out packageId);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                throw PackageManagerErrorFactory.GetException(err, "Failed to get package id fro given package handle.");
            }
            return CreatePackage(packageInfoHandle, packageId);
        }

        private static List<string> GetPackagePrivilegeInformation(IntPtr packageInfoHandle)
        {
            List<string> privileges = new List<string>();
            Interop.Package.PackageInfoPrivilegeInfoCallback privilegeInfoCb = (privilege, userData) =>
            {
                privileges.Add(privilege);
                return true;
            };

            Interop.PackageManager.ErrorCode err = Interop.Package.PackageInfoForeachPrivilegeInfo(packageInfoHandle, privilegeInfoCb, IntPtr.Zero);
            if (err != Interop.PackageManager.ErrorCode.None)
            {
                Log.Warn(LogTag, string.Format("Failed to get privilage info. err = {0}", err));
            }
            return privileges;
        }
    }
}
