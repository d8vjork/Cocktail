using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScintillaNET;
using System.Windows.Forms;
using System.Drawing;

namespace Cocktail
{
    class SuEditor : Scintilla
    {
        private Scintilla editor;
        public bool isInitialized;

        public SuEditor(Scintilla editor)
        {
            this.editor = editor;
            isInitialized = true;

            // Initialize
            InitColors();
            InitSyntaxColoring();
            InitNumberMargin();
            InitBookmarkMargin();
            InitCodeFolding();
            InitHotkeys();
        }

        private void InitColors()
        {
            editor.SetSelectionBackColor(true, IntToColor(0x114D9C));
        }

        private void InitHotkeys()
        {

            // register the hotkeys with the form
            //HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
            //HotKeyManager.AddHotKey(this, OpenFindDialog, Keys.F, true, false, true);
            //HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.R, true);
            //HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.H, true);
            //HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
            //HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
            //HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
            //HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
            //HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);
            //HotKeyManager.AddHotKey(this, CloseSearch, Keys.Escape);

            // remove conflicting hotkeys from scintilla
            editor.ClearCmdKey(Keys.Control | Keys.F);
            editor.ClearCmdKey(Keys.Control | Keys.R);
            editor.ClearCmdKey(Keys.Control | Keys.H);
            editor.ClearCmdKey(Keys.Control | Keys.L);
            editor.ClearCmdKey(Keys.Control | Keys.U);

        }

        private void InitSyntaxColoring()
        {

            // Configure the default style
            editor.StyleResetDefault();
            editor.Styles[Style.Default].Font = "Consolas";
            editor.Styles[Style.Default].Size = 10;
            editor.Styles[Style.Default].BackColor = IntToColor(0x212121);
            editor.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            //editor.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            //editor.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            //editor.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            //editor.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            //editor.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            //editor.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
            //editor.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
            //editor.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            //editor.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            //editor.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            //editor.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            //editor.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            //editor.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            //editor.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            //editor.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            //editor.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            //editor.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            editor.Lexer = Lexer.PhpScript;
        }

        #region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// the background color of the text area
        /// </summary>
        private const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = true;

        private void InitNumberMargin()
        {

            editor.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            editor.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            editor.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            editor.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = editor.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            //editor.MarginClick += editor_MarginClick;
        }

        private void InitBookmarkMargin()
        {

            //editor.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

            var margin = editor.Margins[BOOKMARK_MARGIN];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BOOKMARK_MARKER);
            //margin.Cursor = MarginCursor.Arrow;

            var marker = editor.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(IntToColor(0xFF003B));
            marker.SetForeColor(IntToColor(0x000000));
            marker.SetAlpha(100);

        }

        private void InitCodeFolding()
        {

            editor.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
            editor.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

            // Enable code folding
            editor.SetProperty("fold", "1");
            editor.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            editor.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            editor.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            editor.Margins[FOLDING_MARGIN].Sensitive = true;
            editor.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                editor.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
                editor.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            editor.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            editor.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            editor.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            editor.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            editor.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            editor.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            editor.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            editor.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

        #endregion

        #region Uppercase / Lowercase

        private void Lowercase()
        {
            // save the selection
            int start = editor.SelectionStart;
            int end = editor.SelectionEnd;

            // modify the selected text
            editor.ReplaceSelection(editor.GetTextRange(start, end - start).ToLower());

            // preserve the original selection
            editor.SetSelection(start, end);
        }

        private void Uppercase()
        {

            // save the selection
            int start = editor.SelectionStart;
            int end = editor.SelectionEnd;

            // modify the selected text
            editor.ReplaceSelection(editor.GetTextRange(start, end - start).ToUpper());

            // preserve the original selection
            editor.SetSelection(start, end);
        }

        #endregion

        #region Indent / Outdent

        private void Indent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
            // although the indentation function exists. Pressing TAB with the editor focused confirms this.
            GenerateKeystrokes("{TAB}");
        }

        private void Outdent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
            // although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
            GenerateKeystrokes("+{TAB}");
        }

        private void GenerateKeystrokes(string keys)
        {
            HotKeyManager.Enable = false;
            editor.Focus();
            SendKeys.Send(keys);
            HotKeyManager.Enable = true;
        }

        #endregion

        #region Zoom

        private new void ZoomIn()
        {
            editor.ZoomIn();
        }

        private new void ZoomOut()
        {
            editor.ZoomOut();
        }

        private void ZoomDefault()
        {
            editor.Zoom = 0;
        }


        #endregion

        #region Quick Search Bar

        bool SearchIsOpen = false;

        private void OpenSearch()
        {

            //SearchManager.SearchBox = TxtSearch;
            //SearchManager.editor = editor;

            if (!SearchIsOpen)
            {
                SearchIsOpen = true;
                InvokeIfNeeded(delegate ()
                {
                    //PanelSearch.Visible = true;
                    //TxtSearch.Text = SearchManager.LastSearch;
                    //TxtSearch.Focus();
                    //TxtSearch.SelectAll();
                });
            }
            else
            {
                InvokeIfNeeded(delegate ()
                {
                    //TxtSearch.Focus();
                    //TxtSearch.SelectAll();
                });
            }
        }
        private void CloseSearch()
        {
            if (SearchIsOpen)
            {
                SearchIsOpen = false;
                InvokeIfNeeded(delegate ()
                {
                    //PanelSearch.Visible = false;
                    //CurBrowser.GetBrowser().StopFinding(true);
                });
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            CloseSearch();
        }

        private void BtnPrevSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(false, false);
        }
        private void BtnNextSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(true, false);
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchManager.Find(true, true);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (HotKeyManager.IsHotkey(e, Keys.Enter))
            {
                SearchManager.Find(true, false);
            }
            if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true))
            {
                SearchManager.Find(false, false);
            }
        }

        #endregion

        #region Find & Replace Dialog

        private void OpenFindDialog()
        {
            //
        }
        private void OpenReplaceDialog()
        {
            //
        }

        #endregion

        #region Utils

        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        public void InvokeIfNeeded(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        #endregion
    }
}
