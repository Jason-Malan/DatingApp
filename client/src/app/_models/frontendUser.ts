import { Photo } from './photo';

export interface FrontendUser {
  id: number;
  userName: string;
  knownAs: string;
  age: number;
  created: Date;
  lastActive: Date;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  photos: Photo[];
  photoUrl: string;
}
