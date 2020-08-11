import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
    selector: '[recruInput]'
})

export class RecruInputDirective {

    inputContainer = document.createElement('div');
    label = document.createElement('span');

    @Input() set recruInput(recruInputLabel: string) {
        this.viewContainer.createEmbeddedView(this.elementTemplate);
        this.label.classList.add('recru-input-label');
        this.label.innerText = recruInputLabel;

        const element = this.initializeElement();

        if (!element) {
            console.warn(`[recriInput] Tag inside of ${this.elementTemplate.elementRef.nativeElement.parentElement.nodeName} not registered in recru-input directive`);
            return;
        }

        const elementsMap = {
            'nz-radio-group': this.generateGenericInput.bind(this, 'recru-input-radio-group'),
            'nz-select': this.generateGenericInput.bind(this, 'recru-input-group'),
            'nz-upload': this.generateGenericInput.bind(this, 'recru-input-upload'),
            'nz-date-picker': this.generateGenericInput.bind(this, 'recru-input-group'),
            'nz-month-picker': this.generateGenericInput.bind(this, 'recru-input-group'),
            'nz-slider': this.generateGenericInput.bind(this, 'recru-input-slider'),
            'div': this.generateGenericInput.bind(this, 'recru-input-generic'),
            'nz-input-group': this.generateInputGroup.bind(this),
            'app-text-editor': this.generateTextEditor.bind(this),
            'input': this.generateInput.bind(this)
        };

        elementsMap[element.nodeName.toLowerCase()](element);
    }

    constructor(private elementTemplate: TemplateRef<any>, private viewContainer: ViewContainerRef) {
    }

    private generateGenericInput(className: string, element: any) {
        this.inputContainer.classList.add(className);

        this.inputContainer.appendChild(element);
        this.inputContainer.appendChild(this.label);
        this.elementTemplate.elementRef.nativeElement.parentElement.appendChild(this.inputContainer);
    }

    private generateInput(element: any) {
        this.inputContainer.classList.add('recru-input-group');
        element.setAttribute("placeholder", " ");
        element.classList.add("recru-input-item");

        this.inputContainer.appendChild(element);
        this.inputContainer.appendChild(this.label);
        this.elementTemplate.elementRef.nativeElement.parentElement.appendChild(this.inputContainer);
    }

    private generateInputGroup(element: any) {
        this.inputContainer.classList.add('recru-input-group');
        setTimeout(() => {
            const input = element.querySelector("input.ant-input");
            const subElement = element.querySelector("span.ant-input-wrapper");

            this.inputContainer.classList.add('recru-input-group--phone');
            this.inputContainer.appendChild(input);
            this.inputContainer.appendChild(this.label);
            subElement.appendChild(this.inputContainer);
        }, 100)

        this.elementTemplate.elementRef.nativeElement.parentElement.appendChild(element);
    }

    private generateTextEditor(element: any) {
        const editor = element.querySelector("div.text-editor");
        const editorWrapper = element.querySelector('div#recruEdit-wrapper');

        editor.classList.add("recru-input-text-editor");
        editorWrapper.appendChild(this.label);
    }

    private initializeElement(): any {
        const parentElement = this.elementTemplate.elementRef.nativeElement.parentElement;
        
        return parentElement.querySelector("nz-radio-group")
            || parentElement.querySelector("nz-select")
            || parentElement.querySelector("nz-upload")
            || parentElement.querySelector("nz-date-picker")
            || parentElement.querySelector("nz-month-picker")
            || parentElement.querySelector("nz-input-group")
            || parentElement.querySelector("nz-slider")
            || parentElement.querySelector("app-text-editor")
            || parentElement.querySelector("input")
            || parentElement.querySelector("div")
            || undefined;
    }
}
