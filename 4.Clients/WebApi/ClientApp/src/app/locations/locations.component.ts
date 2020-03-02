import { Component, OnInit } from '@angular/core';
import { Room } from 'src/entities/room';
import { Office } from 'src/entities/office';
import { FacadeService } from '../services/facade.service';
import { AppComponent } from '../app.component';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent implements OnInit {
  tab: string;

  emptyRoom: Room[] = [];
  listOfDisplayDataRoom = [...this.emptyRoom];

  emptyOffice: Office[] = [];
  listOfDisplayDataOffice = [...this.emptyOffice];
  
  constructor(private route: ActivatedRoute, private facade: FacadeService, private app: AppComponent) { }

  ngOnInit() {
    this.getOffices();
    this.getRooms();
    this.tab=this.route.snapshot.params['tab'];
    this.route.params.subscribe((params: Params)=>{
      this.tab=this.route.snapshot.params['tab'];
    });
  }

  getRooms() {
    this.facade.RoomService.get()
    .subscribe(res => {
      this.emptyRoom = res;
      this.listOfDisplayDataRoom = res;
      }, err => {
      console.log(err);
    });
  }  

  getOffices() {
    this.facade.OfficeService.get().subscribe(res => {
      this.emptyOffice = res;
      this.listOfDisplayDataOffice = res;
    }, err => {
      console.log(err);
    })
  }

}
