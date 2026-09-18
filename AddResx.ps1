$en_path = "C:\Repos\040 MauiNet10\Purse\040 Projects\Purse.Shared\Resources\Strings\AppResources.resx"
$de_path = "C:\Repos\040 MauiNet10\Purse\040 Projects\Purse.Shared\Resources\Strings\AppResources.de.resx"
$es_path = "C:\Repos\040 MauiNet10\Purse\040 Projects\Purse.Shared\Resources\Strings\AppResources.es.resx"

if (-not (Test-Path $de_path)) { Copy-Item $en_path $de_path }
if (-not (Test-Path $es_path)) { Copy-Item $en_path $es_path }

$resources = @(
    @("TransactionCountFormat", "{0} transactions", "{0} Transaktionen", "{0} transacciones"),
    @("NoItemsAdded", "No items added yet.", "Noch keine Elemente hinzugefügt.", "Aún no se han añadido elementos."),
    @("AddManuallyBtn", "✍️ Add Manually", "✍️ Manuell hinzufügen", "✍️ Añadir manualmente"),
    @("UploadReceiptBtn", "📁 Upload Receipt", "📁 Beleg hochladen", "📁 Subir recibo"),
    @("ScanReceiptBtn", "📷 Scan Receipt", "📷 Beleg scannen", "📷 Escanear recibo"),
    @("CannotDeleteCategoryTitle", "Cannot Delete Category", "Kategorie kann nicht gelöscht werden", "No se puede eliminar la categoría"),
    @("CannotDeleteCategoryMessageFormat", "This category cannot be deleted because it is currently used in {0} transaction(s).", "Diese Kategorie kann nicht gelöscht werden, da sie derzeit in {0} Transaktion(en) verwendet wird.", "Esta categoría no se puede eliminar porque se usa actualmente en {0} transacción(es)."),
    @("DeleteCategoryTitle", "Delete Category", "Kategorie löschen", "Eliminar categoría"),
    @("DeleteCategoryMessageFormat", "Do you want to delete the category '{0}'?", "Möchten Sie die Kategorie '{0}' löschen?", "¿Desea eliminar la categoría '{0}'?"),
    @("Delete", "Delete", "Löschen", "Eliminar"),
    @("Cancel", "Cancel", "Abbrechen", "Cancelar")
)

function Add-Resx {
    param($path, $name, $val)
    
    $xml = [xml](Get-Content $path -Encoding UTF8)
    $root = $xml.DocumentElement

    $node = $root.SelectSingleNode("data[@name='$name']")
    if ($node) {
        $node.SelectSingleNode("value").InnerXml = $val
    } else {
        $newNode = $xml.CreateElement("data")
        $newNode.SetAttribute("name", $name)
        $newNode.SetAttribute("xml:space", "preserve")
        $valueNode = $xml.CreateElement("value")
        $valueNode.InnerXml = $val
        $newNode.AppendChild($valueNode) | Out-Null
        $root.AppendChild($newNode) | Out-Null
    }

    $xml.Save($path)
}

# First, fix EN by removing all duplicates
$xmlEn = [xml](Get-Content $en_path -Encoding UTF8)
$root = $xmlEn.DocumentElement
$toRemove = @()
foreach ($node in $root.SelectNodes("data")) {
    $name = $node.GetAttribute("name")
    if ($resources | Where-Object { $_[0] -eq $name }) {
        $toRemove += $node
    }
}
foreach ($node in $toRemove) {
    $root.RemoveChild($node) | Out-Null
}
$xmlEn.Save($en_path)

foreach ($res in $resources) {
    Add-Resx $en_path $res[0] $res[1]
    Add-Resx $de_path $res[0] $res[2]
    Add-Resx $es_path $res[0] $res[3]
}

Write-Host "Done"
