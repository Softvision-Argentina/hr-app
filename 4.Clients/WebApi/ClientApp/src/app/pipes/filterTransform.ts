export function filterTransform(items: any[], value, itemProp, itemSubProp?) {
    let result;
    if (!!value) {
        result = items.filter(item => {
            const itemValue = itemSubProp ? item[itemSubProp][itemProp] : item[itemProp];
            value = value.toString().toUpperCase();
            return itemValue.toString().toUpperCase().indexOf(value) !== -1;
        });
    } else {
        result = items;
    }

    if (result.length === 0) {
        return [-1];
    } else {
        return result;
    }
}
