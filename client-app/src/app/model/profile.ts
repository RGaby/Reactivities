import { User } from "./user";

export interface IProfile {
    userName: string;
    displayName: string;
    image?: string;
    bio?: string;
    photos?: Photo[]
}

export class Profile implements IProfile {
    constructor(user: User) {
        this.userName = user.username;
        this.displayName = user.displayName;
        this.image = user.image;
    }

    userName: string;
    displayName: string;
    image?: string;
    bio?: string;
    photos?: Photo[];
}

export interface Photo {
    id: string;
    url: string;
    isMain: boolean;
}


export class ProfileInfoForm {

    constructor(profileForm?: ProfileInfoForm | Profile) {
        if (profileForm) {
            this.displayName = profileForm.displayName;
            this.bio = profileForm.bio;
        }
    }

    displayName: string = "";
    bio?: string;
}
