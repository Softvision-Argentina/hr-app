export class DefaultDate extends Date{
    private _default: Date;
    constructor(){
        super();
        this._default = new Date(1999, 1, 1);
    }
    
    getDefault(){
        return this._default;
    }

    isEqualToDateTime2(dateTime2: Date){
        const dateTime2String = dateTime2.toString().split('T')[0];
        return this._default.toISOString().split('T')[0] === dateTime2String;
    }

}