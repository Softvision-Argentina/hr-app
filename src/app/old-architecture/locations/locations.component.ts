import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Office } from '@shared/models/office.model';
import { Room } from '@shared/models/room.model';
import { FacadeService } from '@shared/services/facade.service';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.scss']
})
export class LocationsComponent implements OnInit {
  tab: string;

  emptyRoom: Room[] = [];
  listOfDisplayDataRoom = [...this.emptyRoom];

  emptyOffice: Office[] = [];
  listOfDisplayDataOffice = [...this.emptyOffice];

  constructor(private route: ActivatedRoute, private facade: FacadeService) { }

  ngOnInit() {
    this.getOffices();
    this.getRooms();
    this.tab = this.route.snapshot.params['tab'];
    this.route.params.subscribe(() => {
      this.tab = this.route.snapshot.params['tab'];
    });
  }

  onOfficesChanged() {
    this.getOffices();
  }
  onRoomsChanged() {
    this.getRooms();
  }

  getRooms() {
    this.facade.RoomService.get()
    .subscribe(res => {
      this.emptyRoom = res;
      this.listOfDisplayDataRoom = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
    });
  }

  getOffices() {
    this.facade.OfficeService.get().subscribe(res => {
      this.emptyOffice = res;
      this.listOfDisplayDataOffice = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
  }

}
