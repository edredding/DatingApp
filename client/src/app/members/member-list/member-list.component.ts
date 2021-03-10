import { Component, OnInit } from '@angular/core';
<<<<<<< HEAD
import { Observable } from 'rxjs';
=======
>>>>>>> a4e7b21418412cfd9ee9cd1ab095c5a5c891240c
import { Member } from 'src/app/_models/members';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
<<<<<<< HEAD
  members$: Observable<Member[]>;
=======
  members: Member[];
>>>>>>> a4e7b21418412cfd9ee9cd1ab095c5a5c891240c

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
<<<<<<< HEAD
    this.members$ = this.memberService.getMembers();
  }
=======
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers().subscribe(members => {
      this.members = members;
    });
  }
>>>>>>> a4e7b21418412cfd9ee9cd1ab095c5a5c891240c
}
