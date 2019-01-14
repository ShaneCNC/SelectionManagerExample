// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UtilityService.cs" company="CNC Software, Inc.">
//   Copyright (c) 2013 CNC Software, Inc.
// </copyright>
// <summary>
//   The utility service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SelectionManagerExample.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Mastercam.BasicGeometry;
    using Mastercam.Curves;
    using Mastercam.Database;
    using Mastercam.Database.Types;
    using Mastercam.IO;
    using Mastercam.IO.Types;
    using Mastercam.Support;
    using Resources;

    /// <summary>
    /// The utility service.
    /// </summary>
    public static class UtilityService
    {
        /// <summary>
        /// The mold base file path.
        /// </summary>
        private static readonly string MoldBase = Path.Combine(SettingsManager.UserDirectory, @"mcx\Moldbase.mcx-8");

        #region Internal Methods

        /// <summary> Opens our test drawing </summary>
        ///
        /// <returns> true if it succeeds, false if it fails. </returns>
        internal static bool Initialize()
        {
            const int ViewNumber = (int)GraphicsViewType.Iso;

            // Start fresh
            FileManager.New(false);

            if (!File.Exists(MoldBase))
            {
                var msg = $"Missing File {MoldBase}. Please copy the included file to the applicable folder.";
                DialogManager.OK(msg, "Missing File");
                return false;
            }

            if (!FileManager.Open(MoldBase))
            {
                return false;
            }

            ViewManager.GraphicsView = SearchManager.GetViews(ViewNumber)[0];
            GraphicsManager.FitScreen();
            GraphicsManager.Repaint(true);
            SelectionManager.UnselectAllGeometry();
            GraphicsManager.ClearColors(new GroupSelectionMask());

            return true;
        }

        /// <summary> Box select geometry. </summary>
        ///
        /// <param name="selectionType"> Type of the selection. </param>
        ///
        /// <returns> true if it succeeds, false if it fails. </returns>
        internal static bool BoxSelectGeometry(BoxSelectionType selectionType)
        {
            return SelectionManager.BoxSelectGeometry(ApplicationStrings.BoxSelection, new GeometryMask(true), selectionType);
        }

        /// <summary> Ask for multiple geometry. </summary>
        ///
        /// <returns> A list of selected geometries. </returns>
        internal static IEnumerable<Geometry> AskForMultipleGeometry()
        {
            return SelectionManager.AskForMultipleGeometry(ApplicationStrings.SelectMultipleGeometry, new GeometryMask(true));
        }

        /// <summary> Executes the ask for arc geometry action. </summary>
        ///
        /// <returns> An Arc Geometry. </returns>
        internal static ArcGeometry AskForArcGeometry()
        {
            // Cast the return Geometry type to an explicit ArcGeometry, this is fine because we are masking on an Arc only
            return AskUser(ApplicationStrings.SelectAnArc, new GeometryMask(false, false, true, false, false, false, false)) as ArcGeometry;
        }

        /// <summary> Executes the ask for line geometry action. </summary>
        ///
        /// <returns> A Line Geometry. </returns>
        internal static LineGeometry AskForLineGeometry()
        {
            // Cast the return Geometry type to  an explicit LineGeometry, this is fine because we are masking on Line only
            return AskUser(ApplicationStrings.SelectALine, new GeometryMask(false, true, false, false, false, false, false)) as LineGeometry;
        }

        /// <summary> Executes the ask for point geometry action. </summary>
        ///
        /// <returns> A PointGeometry. </returns>
        internal static PointGeometry AskForPointGeometry()
        {
            // Cast the return Geometry type to an explicit PointGeometry, this is fine because we are masking on Points only
            return AskUser(ApplicationStrings.SelectAPoint, new GeometryMask(true, false, false, false, false, false, false)) as PointGeometry;
        }

        /// <summary> Executes the ask for geometry action. </summary>
        ///
        /// <returns> A string representing the geometry type. </returns>
        internal static string AskForGeometry()
        {
            // Mask on all geometry types
            var geometry = AskUser(ApplicationStrings.SelectAnyGeometry, new GeometryMask(true));
            if (geometry == null)
            {
                return ApplicationStrings.NothingSelected;
            }

            if (geometry.GetType() == typeof(ArcGeometry))
            {
                return ApplicationStrings.SelectedArc;
            }

            if (geometry.GetType() == typeof(LineGeometry))
            {
                return ApplicationStrings.SelectedLine;
            }

            return geometry.GetType() == typeof(PointGeometry)
                       ? ApplicationStrings.SelectedPoint
                       : ApplicationStrings.UnknownGeometry;
        }

        #endregion

        #region Private Methods

        /// <summary> Initialises this object. </summary>
        ///
        /// <param name="prompt"> The prompt. </param>
        /// <param name="mask">   The mask. </param>
        ///
        /// <returns> A Geometry. </returns>
        private static Geometry AskUser(string prompt, GeometryMask mask)
        {
            return SelectionManager.AskForGeometry(prompt, mask);
        }

        #endregion
    }
}
