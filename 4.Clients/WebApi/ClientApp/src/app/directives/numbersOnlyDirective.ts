import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: 'input[numbersOnly]'
})

export class NumbersOnlyDirective {

    constructor(private element: ElementRef) { }

    @HostListener('keydown', ['$event']) onkeydown(element: KeyboardEvent) {
        if (
            // If allowed characters: Delete, Backspace, Tab, Escape, Enter, or
            [46, 8, 9, 27, 13].indexOf(element.keyCode) !== -1 ||
            (element.keyCode === 65 && element.ctrlKey === true) || // Ctrl+A
            (element.keyCode === 67 && element.ctrlKey === true) || // Ctrl+C
            (element.keyCode === 86 && element.ctrlKey === true) || // Ctrl+V
            (element.keyCode === 88 && element.ctrlKey === true) || // Ctrl+X
            (element.keyCode === 65 && element.metaKey === true) || // Cmd+A (Mac)
            (element.keyCode === 67 && element.metaKey === true) || // Cmd+C (Mac)
            (element.keyCode === 86 && element.metaKey === true) || // Cmd+V (Mac)
            (element.keyCode === 88 && element.metaKey === true) || // Cmd+X (Mac)
            (element.keyCode >= 35 && element.keyCode <= 39) // Home, End, Left, Right
        ) {
            return;
        }
        // Otherwise prevent if not a number

        if ((element.keyCode < 96 || element.keyCode > 105) && (element.shiftKey || (element.keyCode < 48 || element.keyCode > 57))        ) {
            element.preventDefault();
        }
    }

    @HostListener('paste', ['$event'])
    onPaste(event: ClipboardEvent) {
        event.preventDefault();
        const pastedInput: string = event.clipboardData
            .getData('text/plain')
            .replace(/\D/g, '');
        document.execCommand('insertText', false, pastedInput);
    }

}
