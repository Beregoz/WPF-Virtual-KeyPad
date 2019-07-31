# WPF-Virtual-KeyPad
Virtual key pad in WPF

Reference the lib in your project and use by attaching the control like this:
```xml
<TextBox virtualKeyPad:VirtualKeyPad.Mode="Touch" />
```
To add new buttons to the template just edit/override the template at Themes/generic.xaml
```xml
<Button CommandParameter="{x:Static Key.NumPad2}">
    <TextBlock Text="{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type Button}}, Path=CommandParameter, Converter={StaticResource KeyToCharConverter}}"/>
</Button>
```
This would result in "NumPad 2".

All available characters can be found in [System.Windows.Input.Key](https://referencesource.microsoft.com/#WindowsBase/Base/System/Windows/Input/Key.cs) 
