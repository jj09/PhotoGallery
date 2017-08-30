using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using MWPhotoBrowser;
using Photos;
using UIKit;

namespace PhotoGallery
{
    public partial class FirstViewController : UITableViewController
    {
        private BrowserDelegate browserDelegate;

        private List<Tuple<string, string>> menuItems = new List<Tuple<string, string>>();

        public FirstViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            menuItems.AddRange(new[] {
                new Tuple<string, string> ("Multiple photo grid", "showing grid first, nav arrows enabled"),
            });

            Title = "MWPhotoBrowser";

            browserDelegate = new BrowserDelegate(TableView);

            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Bordered, null);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return 1;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // Create
            var cell = TableView.DequeueReusableCell("Cell");
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "Cell");
            }
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            // Configure
            var tuple = menuItems[indexPath.Row];
            cell.TextLabel.Text = tuple.Item1;
            cell.DetailTextLabel.Text = tuple.Item2;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var browser = new PhotoBrowser(browserDelegate);
            browser.DisplayActionButton = true;
            browser.DisplaySelectionButtons = false;
            browser.DisplayNavArrows = false;
            browser.EnableGrid = true;
            browser.StartOnGrid = false;
            browser.AutoPlayOnAppear = false;

            
            browserDelegate.ShowMultiplePhotoGrid(browser);

            // Settings
            browser.EnableSwipeToDismiss = false;
            browser.CurrentIndex = 0;

            // Show
            NavigationController.PushViewController(browser, true);

            // Deselect
            TableView.DeselectRow(indexPath, true);
        }

        private class BrowserDelegate : PhotoBrowserDelegate
        {
            private UITableView tableView;

            private bool[] _selections;

            private List<IPhoto> photos = new List<IPhoto>();
            private List<IPhoto> thumbs = new List<IPhoto>();

            public BrowserDelegate(UITableView tableView)
            {
                this.tableView = tableView;
            }

            public override nuint GetPhotoCount(PhotoBrowser photoBrowser)
            {
                return (nuint)photos.Count;
            }

            public override IPhoto GetPhoto(PhotoBrowser photoBrowser, nuint index)
            {
                return photos[(int)index];
            }

            public override IPhoto GetThumbnail(PhotoBrowser photoBrowser, nuint index)
            {
                return thumbs[(int)index];
            }

            public override bool IsPhotoSelected(PhotoBrowser photoBrowser, nuint index)
            {
                return _selections[index];
            }

            public override void OnSelectedChanged(PhotoBrowser photoBrowser, nuint index, bool selected)
            {
                _selections[index] = selected;
                Console.WriteLine("Photo at index {0} selected ? {1}.", index, selected);
            }

            public override void DidDisplayPhoto(PhotoBrowser photoBrowser, nuint index)
            {
                Console.WriteLine("Did start viewing photo at index {0}.", index);
            }

            public void ShowMultiplePhotoGrid(PhotoBrowser browser)
            {
                photos = new List<IPhoto>();
                thumbs = new List<IPhoto>();

                PhotoBrowserPhoto photo;

                // Photos

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo5", "jpg"));
                photo.Caption = "White Tower";
                photos.Add(photo);

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo2", "jpg"));
                photo.Caption = "The London Eye is a giant Ferris wheel situated on the banks of the River Thames, in London, England.";
                photos.Add(photo);

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo3", "jpg"));
                photo.Caption = "York Floods";
                photos.Add(photo);

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo4", "jpg"));
                photo.Caption = "Campervan";
                photos.Add(photo);

                // Thumbs

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo5t", "jpg"));
                thumbs.Add(photo);

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo2t", "jpg"));
                thumbs.Add(photo);

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo3t", "jpg"));
                thumbs.Add(photo);

                photo = PhotoBrowserPhoto.FromFilePath(NSBundle.MainBundle.PathForResource("photo4t", "jpg"));
                thumbs.Add(photo);

                browser.StartOnGrid = true;
                browser.DisplayNavArrows = true;
            }
        }
    }
}
