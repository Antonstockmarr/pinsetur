import { NewTrip } from "@/Models/NewTrip";
import { Trip } from "@/Models/Trip";
import { User } from "@/Models/User";
import { Image } from "@/Models/Image";

export function emptyUser() {
    return {
        userName: "",
        name: "",
        role: "User",
        resetPassword: true
    } as User
}

export function emptyImage() {
    return {
        id: -1,
        year: 0,
        uri: "",
        description: "",
        uploadedBy: ""
    } as Image
}

export function emptyTrip() {
    return {
        year: new Date().getFullYear(),
        location: "",
        address: "",
        description: "",
        locationImageId: null,
        coverImageId: null,
        startDate: "",
        endDate: ""    
    } as Trip
}

export function newTrip() {
    return {
        year: new Date().getFullYear(),
        location: "",
        address: "",
        description: ""
    } as NewTrip
}