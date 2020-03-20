export function filterTransform(items: any[], value, itemProp) {
    let result;
    if(!!value) {
        result = items.filter(item =>{
            const itemValue = item[itemProp].toString().toUpperCase();
            value = value.toString().toUpperCase();
            return itemValue.indexOf(value) !== -1;
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
