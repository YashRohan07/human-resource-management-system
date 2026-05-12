import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  imports: [CommonModule],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.scss'
})
export class ConfirmDialogComponent {

  @Input() title = 'Confirm Action';
  @Input() message = 'Are you sure?';

  @Output() confirmed = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  // User confirmed the action
  confirm(): void {
    this.confirmed.emit();
  }

  // User cancelled the action
  cancel(): void {
    this.cancelled.emit();
  }
}
