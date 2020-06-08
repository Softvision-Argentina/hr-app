import { Component, OnInit, ViewChild, ElementRef, Output, Input, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-text-editor',
  templateUrl: './text-editor.component.html',
  styleUrls: ['./text-editor.component.scss']
})
export class TextEditorComponent implements OnInit {
  @ViewChild('url') url: ElementRef;
  @ViewChild('editor') editor: ElementRef;
  @ViewChild('toolbar') toolbar: ElementRef<HTMLElement>;
  @ViewChild('colorContainer') colorContainer: ElementRef<HTMLElement>;
  
  @Input() setContent: string;
  @Output() getContent: EventEmitter<string> = new EventEmitter();

  isActive: boolean = false;
  isToolbarActive: boolean = true;
  isColorContainerActive: boolean = false;
  isCreateLinkModalVisible: boolean = false;
  urlProtocol: string = "http://";
  colorContainerLeftOffset: number;

  customSelection = {
    indexStart: 0,
    indexEnd: 0,
    length: 0,
    range: new Range()
  };

  toolbarButtons = [
    { name: "Bold", tagName: "bold", enabled: true, icon: "bold" },
    { name: "Italic", tagName: "italic", enabled: true, icon: "italic" },
    { name: "Underline", tagName: "underline", enabled: true, icon: "underline" },
    { name: "Highlight", tagName: "span", enabled: true, icon: "highlight" },
    { name: "Create link", tagName: "a", enabled: true, icon: "link" },
    { name: "Remove link", tagName: "", enabled: true, icon: "link" }
  ];

  colors = [
    { name: "None", hexCode: "transparent" },
    { name: "Red", hexCode: "#ff0000" },
    { name: "Orange", hexCode: "#ff8000" },
    { name: "Yellow", hexCode: "#ffff00" },
    { name: "Green", hexCode: "#00ff00" },
    { name: "Cyan", hexCode: "#00ffff" },
    { name: "Purple", hexCode: "#7f00ff" }
  ];

  constructor(
  ) { }

  ngOnInit() {
    this.wrapperStyle();
    this.editor.nativeElement.innerHTML = this.setContent;
    this.followLinkHandler();
  }

  wrapperStyle() {
    let withinWrapper = document.querySelectorAll(".recruEdit-editor, .recruEdit-toolbar");

    for (var i = 0; i < withinWrapper.length; i++) {
      let wrapper = document.querySelector(".recruEdit-wrapper");
      withinWrapper[i].addEventListener("focus", () => {
        wrapper.classList.add("focus");
      });
      withinWrapper[i].addEventListener("blur", () => {
        wrapper.classList.remove("focus");
      });
    }
  }

  toolHandler(toolName: string, tagName: string) {
    this.getSelection();

    switch (toolName) {
      case "Bold":
        this.basicElementHandler(tagName);
        break;
      case "Italic":
        this.basicElementHandler(tagName);
        break;
      case "Underline":
        this.basicElementHandler(tagName);
        break;
      case "Highlight":
        this.hightlightHandler();
        break;
      case "Create link":
        this.showCreateLinkModal();
        break;
      case "Remove link":
        this.removeLink();
        break;
    }

    this.saveEditor();
  }

  basicElementHandler(tagName: string): void {
    document.execCommand(tagName);

    //THIS IS FOR THE NEW FUNCTIONALITY REPLACING EXECCOMAND
    // const rootElement = document.getElementById("recruEdit-editor");
    // let rootChildren = rootElement.children;

    // for(let i = 0; i < rootChildren.length; i++){
    //   this.customSelection.range.startContainer.textContent
    // }

    // let element = document.createElement(tagName);
    // //element.innerText = this.customSelection.range.toString();

    // this.customSelection.range.surroundContents(element);
    //this.customSelection.range.insertNode(element);
  }

  hightlightHandler(): void {
    let hightlightButton = this.toolbar.nativeElement.children['Highlight'];
    this.colorContainerLeftOffset = hightlightButton.offsetLeft;
    this.isColorContainerActive = !this.isColorContainerActive;
  }

  changeBackgoundColor(color: string): void {
    document.execCommand('backColor', false, color);

    //THIS IS FOR THE NEW FUNCTIONALITY REPLACING EXECCOMAND
    // let element = document.createElement("span");
    // element.innerText = this.customSelection.range.toString();
    // element.style.backgroundColor = color;

    // this.customSelection.range.deleteContents();
    // this.customSelection.range.insertNode(element);

    this.isColorContainerActive = false;
    this.saveEditor();
  }

  showCreateLinkModal(): void {
    this.isCreateLinkModalVisible = !this.isCreateLinkModalVisible;
    setTimeout(() => {
      this.url.nativeElement.focus();
    }, 50);
  }

  createLinkModalHandleOk() {
    this.isCreateLinkModalVisible = false;
    this.createLink();
  }

  createLinkModalHandleCancel() {
    this.isCreateLinkModalVisible = false;
  }

  createLink() {
    let fullURL = this.url.nativeElement.value;

    //THIS IS FOR THE NEW FUNCTIONALITY REPLACING EXECCOMAND
    // let anchor = document.createElement("a");
    // anchor.href = fullURL;
    // anchor.target = "_blank";
    // anchor.innerText = this.customSelection.range.toString();
    // anchor.title = fullURL + "\nCTRL + click to follow link";

    this.selectPreviousRange();

    let newAnchorTag = `<a href="${fullURL}" title="${fullURL}\nCTRL + click to follow link">${this.customSelection.range.toString()}</a> `;
    document.execCommand('insertHTML', false, newAnchorTag);

    this.followLinkHandler();
    this.resetUrlField();
    this.saveEditor();
  }

  removeLink() {
    document.execCommand('unlink');
  }

  resetUrlField() {
    this.url.nativeElement.value = 'http://'
  }

  saveEditor() {
    this.getContent.emit(this.editor.nativeElement.innerHTML);
  }

  followLinkHandler() {
    let anchors = document.querySelectorAll("#recruEdit-editor a");

    let isCtrlPressed: boolean = false;
    document.addEventListener('keydown', (e: KeyboardEvent) => {
      isCtrlPressed = e.code == 'ControlLeft';
    });

    document.addEventListener('keyup', (e: KeyboardEvent) => {
      isCtrlPressed = false;
    });

    anchors.forEach((element, key) => {
      element.addEventListener('click', newLocation);

      function newLocation(e: MouseEvent) {
        if (isCtrlPressed) {
          window.open(element.getAttribute('href'), '_blank');
          isCtrlPressed = false;
        }
      }
    });
  }

  getSelection() {
    let selection = window.getSelection();
    if (selection.type != 'None') {
      this.customSelection.indexStart = selection.anchorOffset;
      this.customSelection.indexEnd = selection.focusOffset;
      this.customSelection.length = selection.toString().length;
      this.customSelection.range = selection.getRangeAt(0);
    }
  }

  selectPreviousRange() {
    let selection = window.getSelection();
    selection.removeAllRanges();
    selection.addRange(this.customSelection.range);
  }
}
