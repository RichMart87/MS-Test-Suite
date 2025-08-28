using OpenQA.Selenium;
using SeleniumMStestProject.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Interface
{
    public interface IDriver
    {
        IWebDriver Driver { get; }
        string BaseUrl { get; set; }
        string CurrentUrl { get; }
        string Title { get; }
        string PageSource { get; }
        string WindowHandle { get; }
        string PageName { get; set; }
        string PageTitle { get; set; }
        string PageUrl { get; set; }

        void AcceptAlert();

        void DismissAlert();

        void SwitchToAlert();

        void SwitchToDefaultContent();

        void SwitchToFrame(string frameName);

        void SwitchToFrame(int frameIndex);

        void SwitchToFrame(IWebElement frameElement);

        void SwitchToWindow(string windowName);

        void SwitchToWindow(int windowIndex);

        void SwitchToWindow(IWebElement windowElement);

        void SwitchToParentFrame();

        void SwitchToNewWindow();

        void SwitchToNewWindow(string windowName);

        void SwitchToNewWindow(int windowIndex);

        void SwitchToNewWindow(IWebElement windowElement);

        void SwitchToNewWindow(string windowName, string parentWindowName);

        void SwitchToNewWindow(int windowIndex, int parentWindowIndex);

        void SwitchToNewWindow(IWebElement windowElement, IWebElement parentWindowElement);

        string AlertText { get; }

        void Click(IWebElement element);

        void Click(string elementId);

        //void Click(Control control);

        void Close();

        void Quit();

        void CloseTab();

        void NavigateTo(string url);

        List<string> GetWindowHandles();

        List<string> GetWindowHandles(string windowName);

        //List<string> GetSelectOptions(Control control,bool getTextValue = true);

        //void Hover(Control control);
    }
}