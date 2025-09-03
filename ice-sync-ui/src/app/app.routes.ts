import { Routes } from '@angular/router';
import { WorkflowsComponent } from './components/workflows/workflows';

export const routes: Routes = [
  { path: '', component: WorkflowsComponent },
  { path: '**', redirectTo: '' } 
];

