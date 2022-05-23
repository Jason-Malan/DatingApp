import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  maxDate: Date;

  get usernameControl() {
    return this.registerForm.get('username') as FormControl;
  }

  get passwordControl() {
    return this.registerForm.get('password') as FormControl;
  }

  get confirmPasswordControl() {
    return this.registerForm.get('confirmPassword') as FormControl;
  }

  get genderControl() {
    return this.registerForm.get('gender') as FormControl;
  }

  get knownAsControl() {
    return this.registerForm.get('knownAs') as FormControl;
  }

  get dateOfBirthControl() {
    return this.registerForm.get('dateOfBirth') as FormControl;
  }

  get cityControl() {
    return this.registerForm.get('city') as FormControl;
  }

  get countryControl() {
    return this.registerForm.get('country') as FormControl;
  }

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private route: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: [
        '',
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(
      (response) => {
        this.route.navigateByUrl('/members');
      },
      (error) => {}
    );
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
