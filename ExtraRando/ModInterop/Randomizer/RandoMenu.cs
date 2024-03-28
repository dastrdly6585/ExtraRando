using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using System.Linq;

namespace ExtraRando.ModInterop.Randomizer;

internal class RandoMenu
{
    #region Members

    private static RandoMenu _instance;
    private MenuPage _connectionPage;
    private MenuElementFactory<RandoSettings> _elementFactory;
    private GridItemPanel _hintPanel;

    #endregion

    #region Properties

    public static RandoMenu Instance => _instance ??= new();

    #endregion

    #region Event handler

    private void ConstructMenu(MenuPage previousPage)
    {
        // Generate pages and setting elements
        _connectionPage = new("Extra Rando", previousPage);
        _elementFactory = new(_connectionPage, ExtraRando.Instance.Settings);
        _hintPanel = new(_connectionPage, new(0, -275), 4, 0, 300, false, new IMenuElement[] { 
            _elementFactory.ElementLookup["JunkItemHints"],
            _elementFactory.ElementLookup["PotentialItemHints"],
            _elementFactory.ElementLookup["UsefulItemHints"],
            _elementFactory.ElementLookup["RandomLocationHints"]});

        VerticalItemPanel leftPanel = new(_connectionPage, new(0, 0), 80f, false, new IMenuElement[] {
            _elementFactory.ElementLookup["RandomizeHotSprings"],
            _elementFactory.ElementLookup["RandomizeColoAccess"],
            _elementFactory.ElementLookup["RandomizePantheonAccess"],
            _elementFactory.ElementLookup["RandomizeButt"],
            _elementFactory.ElementLookup["RandomizeAwfulLocations"]
        });
        VerticalItemPanel rightPanel = new(_connectionPage, new(0, 0), 80f, false, new IMenuElement[] {
            _elementFactory.ElementLookup["SplitFireball"],
            _elementFactory.ElementLookup["SplitShadeCloak"],
            _elementFactory.ElementLookup["UseKeyring"],
            _elementFactory.ElementLookup["ScarceItemPool"],
            _elementFactory.ElementLookup["BlockEarlyGameStags"],
            _elementFactory.ElementLookup["AddFixedHints"],
            _elementFactory.ElementLookup["AddHintMarkers"]
        });
        
        GridItemPanel optionPanels = new(_connectionPage, new(0f, 150), 2, 100f, 600f, false, new IMenuElement[] { leftPanel, rightPanel });
        new VerticalItemPanel(_connectionPage, new(0, 450f), 150f, true, new IMenuElement[] { _elementFactory.ElementLookup["Enabled"], optionPanels  });
        _elementFactory.ElementLookup["NoLogic"].MoveTo(new(0, -350f));

        _elementFactory.ElementLookup["AddHintMarkers"].SelfChanged += ToggleHintVisibility;
        if (!ExtraRando.Instance.Settings.AddHintMarkers)
            _hintPanel.Hide();
        else
            _hintPanel.Show();
    }

    private void ToggleHintVisibility(IValueElement obj)
    {
        if ((bool)obj.Value)
            _hintPanel.Show();
        else
            _hintPanel.Hide();
    }

    private bool HandleButton(MenuPage previousPage, out SmallButton connectionButton)
    {
        SmallButton button = new(previousPage, "Extra Rando");
        button.AddHideAndShowEvent(previousPage, _connectionPage);
        _connectionPage.BeforeGoBack += () => button.Text.color = !ExtraRando.Instance.Settings.Enabled ? Colors.DEFAULT_COLOR : Colors.TRUE_COLOR;
        button.Text.color = !ExtraRando.Instance.Settings.Enabled ? Colors.DEFAULT_COLOR : Colors.TRUE_COLOR;
        connectionButton = button;
        return true;
    }

    #endregion

    #region Methods

    internal static void Initialize()
    {
        RandomizerMenuAPI.AddMenuPage(Instance.ConstructMenu, Instance.HandleButton);
        MenuChangerMod.OnExitMainMenu += () => _instance = null;
    }

    internal void PassSettings(RandoSettings randoSettings)
    {
        if (randoSettings == null)
            _elementFactory.ElementLookup[nameof(ExtraRando.Instance.Settings.Enabled)].SetValue(false);
        else
            _elementFactory.SetMenuValues(randoSettings);
    }

    #endregion
}
