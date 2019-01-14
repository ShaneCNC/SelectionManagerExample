// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NethookMain.cs" company="CNC Software, Inc.">
//   Copyright (c) 2013 CNC Software, Inc.
// </copyright>
// <summary>
//   Describes this class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SelectionManagerExample
{
    using System.Linq;

    using Mastercam.App;
    using Mastercam.App.Types;
    using Mastercam.IO;
    using Mastercam.IO.Types;
    using Mastercam.Support;

    using Resources;
    using Services;

    /// <summary>
    /// Describes this class.
    /// </summary>
    public class NethookMain : NetHook3App
    {
        #region Public Override Methods

        /// <summary>
        /// The main entry point for your NETHook.
        /// </summary>
        /// <param name="param">System parameter.</param>
        /// <returns>A <c>MCamReturn</c> return type representing the outcome of your NetHook application.</returns>
        public override MCamReturn Run(int param)
        {
            if (UtilityService.Initialize())
            {
                // AskForPointGeometry
                var point = UtilityService.AskForPointGeometry();
                DialogManager.OK(
                    point != null ? ApplicationStrings.SelectedPoint : ApplicationStrings.NothingSelected,
                    ApplicationStrings.SelectAPoint);

                this.Reset();

                // AskForArcGeometry
                var arc = UtilityService.AskForArcGeometry();
                DialogManager.OK(
                    arc != null ? ApplicationStrings.SelectedArc : ApplicationStrings.NothingSelected,
                    ApplicationStrings.SelectAnArc);

                this.Reset();

                // AskForLineGeometry
                var line = UtilityService.AskForLineGeometry();
                DialogManager.OK(
                    line != null ? ApplicationStrings.SelectedLine : ApplicationStrings.NothingSelected,
                    ApplicationStrings.SelectALine);

                this.Reset();

                // AskForGeometry
                var geometryType = UtilityService.AskForGeometry();
                DialogManager.OK(geometryType, ApplicationStrings.AskForGeometry);

                this.Reset();

                // AskForMultipleGeometry
                var geometries = UtilityService.AskForMultipleGeometry();
                DialogManager.OK(
                    geometries.Any() ? ApplicationStrings.SelectionSuccessful : ApplicationStrings.NothingSelected,
                    ApplicationStrings.SelectMultipleGeometry);

                this.Reset();

                // BoxSelectGeometry Inside
                if (!UtilityService.BoxSelectGeometry(BoxSelectionType.Inside))
                {
                    DialogManager.OK(ApplicationStrings.NothingSelected, ApplicationStrings.BoxSelectionInside);
                }
                else
                {
                    var selectionCount = SearchManager.GetSelectedGeometry().Count();
                    DialogManager.OK(ApplicationStrings.BoxSelectionSuccessful + "\n" + ApplicationStrings.CountOfSelection + ": " + selectionCount, ApplicationStrings.BoxSelectionInside);
                }

                DialogManager.OK(ApplicationStrings.ExampleComplete, string.Empty);
            }
            else
            {
                DialogManager.Error(ApplicationStrings.MissingDrawing, ApplicationStrings.AskForGeometry);
            }

            return MCamReturn.NoErrors;
        }

        #endregion

        #region Private Methods

        /// <summary> Resets the current selection if there is any. </summary>
        private void Reset()
        {
            if (SearchManager.GetSelectedGeometry().Any())
            {
                SelectionManager.UnselectAllGeometry();
            }
        }

        #endregion
    }
}
