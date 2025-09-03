import { Component, OnInit } from '@angular/core';
import { WorkflowService, Workflow } from '../../services/workflow';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-workflows',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './workflows.html',
  styleUrls: ['./workflows.scss']
})
export class WorkflowsComponent implements OnInit {
  workflows: Workflow[] = [];
  loading = false;
  message = '';

  constructor(private workflowService: WorkflowService) {}

  ngOnInit(): void {
    this.loadWorkflows();
  }

  loadWorkflows(): void {
    this.loading = true;
    this.workflowService.getWorkflows().subscribe({
      next: (data) => {
        this.workflows = data;
        this.loading = false;
      },
      error: (err) => {
        this.message = err;
        this.loading = false;
      }
    });
  }

  runWorkflow(id: number): void {
    this.workflowService.runWorkflow(id).subscribe({
      next: () => {
        this.message = `Workflow ${id} started successfully.`;
      },
      error: (err) => {
        this.message = err;
      }
    });
  }
}
