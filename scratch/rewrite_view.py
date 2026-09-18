import re

with open("040 Projects/Purse/View/Transactions/TransactionDetailView.xaml", "r", encoding="utf-8") as f:
    content = f.read()

# Add xmlns:components
content = re.sub(
    r'xmlns:desktop="clr-namespace:Purse.View.Desktop"',
    r'xmlns:desktop="clr-namespace:Purse.View.Desktop"\n             xmlns:components="clr-namespace:Purse.View.Components"',
    content
)

# Replace the vendor picker
picker_regex = r'<Picker\s*Grid\.Row="1"\s*ItemsSource="\{x:Binding Vendors\}"\s*SelectedItem="\{x:Binding SelectedVendor\}"\s*/>'
vendor_overlay_trigger = """<Border Grid.Row="1" Stroke="{x:AppThemeBinding Light={x:StaticResource Gray300}, Dark={x:StaticResource Gray700}}" StrokeThickness="1" BackgroundColor="{x:AppThemeBinding Light={x:StaticResource OffWhite}, Dark={x:StaticResource OffBlack}}">
                        <Border.GestureRecognizers>
                            <TapGestureRecognizer Command="{Binding OpenVendorOverlayCommand}" />
                        </Border.GestureRecognizers>
                        <Label Text="{Binding SelectedVendor, TargetNullValue={x:Static AppResources.SelectVendor}}" Margin="10,0" VerticalOptions="Center" FontFamily="{x:Static FontDefinitions.DefaultMediumFontFamily}" FontSize="16" />
                    </Border>"""
content = re.sub(picker_regex, vendor_overlay_trigger, content)

# Remove the IsOverlayVisible Grid
# We need to find the <Grid BackgroundColor="#A0000000" IsVisible="{Binding IsCategoryOverlayVisible}" ZIndex="100">
# Wait, it might be named IsOverlayVisible or IsCategoryOverlayVisible.
overlay_regex = r'<Grid\s*BackgroundColor="#A0000000"\s*IsVisible="\{Binding IsOverlayVisible\}".*?ZIndex="100">.*?</Grid>'
# Since regex for nested HTML/XML is hard, let's just find the exact string.
start_idx = content.find('<Grid BackgroundColor="#A0000000"')
if start_idx != -1:
    # find the matching </Grid>
    depth = 0
    end_idx = -1
    pos = start_idx
    while pos < len(content):
        next_open = content.find('<Grid', pos + 1)
        next_close = content.find('</Grid>', pos)
        
        if next_close == -1: break
        
        if next_open != -1 and next_open < next_close:
            depth += 1
            pos = next_open
        else:
            depth -= 1
            pos = next_close + 7
            if depth == 0:
                end_idx = pos
                break
    
    if end_idx != -1:
        content = content[:start_idx] + "<components:CategorySelectionView />\n        <components:VendorSelectionView />" + content[end_idx:]

with open("040 Projects/Purse/View/Transactions/TransactionDetailView.xaml", "w", encoding="utf-8") as f:
    f.write(content)

print("Done")
