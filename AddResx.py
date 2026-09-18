import xml.etree.ElementTree as ET
import os
import shutil

en_path = r"040 Projects\Purse.Shared\Resources\Strings\AppResources.resx"
de_path = r"040 Projects\Purse.Shared\Resources\Strings\AppResources.de.resx"
es_path = r"040 Projects\Purse.Shared\Resources\Strings\AppResources.es.resx"

# Copy base resx if localized ones don't exist
if not os.path.exists(de_path):
    shutil.copy(en_path, de_path)
if not os.path.exists(es_path):
    shutil.copy(en_path, es_path)

resources = [
    ("TransactionCountFormat", "{0} transactions", "{0} Transaktionen", "{0} transacciones"),
    ("NoItemsAdded", "No items added yet.", "Noch keine Elemente hinzugefügt.", "Aún no se han añadido elementos."),
    ("AddManuallyBtn", "✍️ Add Manually", "✍️ Manuell hinzufügen", "✍️ Añadir manualmente"),
    ("UploadReceiptBtn", "📁 Upload Receipt", "📁 Beleg hochladen", "📁 Subir recibo"),
    ("ScanReceiptBtn", "📷 Scan Receipt", "📷 Beleg scannen", "📷 Escanear recibo"),
    ("CannotDeleteCategoryTitle", "Cannot Delete Category", "Kategorie kann nicht gelöscht werden", "No se puede eliminar la categoría"),
    ("CannotDeleteCategoryMessageFormat", "This category cannot be deleted because it is currently used in {0} transaction(s).", "Diese Kategorie kann nicht gelöscht werden, da sie derzeit in {0} Transaktion(en) verwendet wird.", "Esta categoría no se puede eliminar porque se usa actualmente en {0} transacción(es)."),
    ("DeleteCategoryTitle", "Delete Category", "Kategorie löschen", "Eliminar categoría"),
    ("DeleteCategoryMessageFormat", "Do you want to delete the category '{0}'?", "Möchten Sie die Kategorie '{0}' löschen?", "¿Desea eliminar la categoría '{0}'?"),
    ("Delete", "Delete", "Löschen", "Eliminar"),
    ("Cancel", "Cancel", "Abbrechen", "Cancelar")
]

def add_res(path, name, val):
    tree = ET.parse(path)
    root = tree.getroot()
    # Check if exists
    for data in root.findall('data'):
        if data.get('name') == name:
            data.find('value').text = val
            tree.write(path, encoding="utf-8", xml_declaration=True)
            return
            
    data = ET.SubElement(root, 'data')
    data.set('name', name)
    data.set('xml:space', 'preserve')
    value = ET.SubElement(data, 'value')
    value.text = val
    tree.write(path, encoding="utf-8", xml_declaration=True)

for res in resources:
    add_res(en_path, res[0], res[1])
    add_res(de_path, res[0], res[2])
    add_res(es_path, res[0], res[3])

print("Done")
