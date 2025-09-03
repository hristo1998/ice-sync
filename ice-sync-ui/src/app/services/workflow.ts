import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Workflow {
  id: number;
  name: string;
  isActive: boolean;
  multiExecBehavior: string;
}

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {
  private apiUrl = `${environment.apiUrl}/workflows`;

  constructor(private http: HttpClient) {}

  getWorkflows(): Observable<Workflow[]> {
    return this.http.get<Workflow[]>(this.apiUrl)
      .pipe(catchError(this.handleError));
  }

  runWorkflow(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/run`, {})
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    let msg = 'An unknown error occurred';
    if (error.error instanceof ErrorEvent) {
      msg = `Client error: ${error.error.message}`;
    } else {
      msg = `Server error ${error.status}: ${error.message}`;
    }
    return throwError(() => msg);
  }
}
