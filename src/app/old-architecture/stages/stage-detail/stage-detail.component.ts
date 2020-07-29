import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Stage } from '@shared/models/stage.model';
import { FacadeService } from '@shared/services/facade.service';

@Component({
  selector: 'app-stage-detail',
  templateUrl: './stage-detail.component.html',
  styleUrls: ['./stage-detail.component.scss']
})

export class StageDetailComponent implements OnInit {

  constructor(private route: ActivatedRoute, private facade: FacadeService, private router: Router) { }

  stage: Stage = null;
  processId: number;

  ngOnInit() {
    this.getStageByID(this.route.snapshot.params['id']);
  }

  getStageByID(id) {
    this.facade.stageService.getByID(id)
        .subscribe(res => {
          this.stage = res;
          this.processId = this.stage.processId;
        },
        // TODO: change this log for a message or delete it
        err => {
          console.log(err);
          });
    this.facade.appService.stopLoading();
  }
}
