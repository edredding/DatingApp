import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
<<<<<<< HEAD
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
=======
>>>>>>> a4e7b21418412cfd9ee9cd1ab095c5a5c891240c
import { environment } from 'src/environments/environment';
import { Member } from '../_models/members';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
<<<<<<< HEAD
  members: Member[] = [];
=======
>>>>>>> a4e7b21418412cfd9ee9cd1ab095c5a5c891240c

  constructor(private http: HttpClient) { }

  getMembers() {
<<<<<<< HEAD
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(username: string) {
    const member = this.members.find(x => x.username === username);
    if (member != undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
=======
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }
>>>>>>> a4e7b21418412cfd9ee9cd1ab095c5a5c891240c
}
