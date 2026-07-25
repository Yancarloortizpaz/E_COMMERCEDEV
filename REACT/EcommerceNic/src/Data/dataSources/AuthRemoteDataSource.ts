import { Platform } from "react-native";

const API_URL =
  Platform.OS === "android"
    ? "http://10.0.2.2:5092"
    : "http://127.0.0.1:5092";

export interface LoginResponse {

    codigo:number;

    msj:string;

    data:{

        token:string;

        userFullName:string;

        userEmail:string;

        userId:number;

    }

}

export interface RegisterResponse{
    codigo:number;
    msj:string;
    templateId:number;
}

class AuthRemoteDataSource{

    async login(email:string,password:string):Promise<LoginResponse>{

        const response=await fetch(`${API_URL}/api/Users/login`,{
            
            method:"POST",

            headers:{

                "Content-Type":"application/json"

            },

            body:JSON.stringify({

                userEmail:email,

                userPasswordPlain:password

            })

        });

        const data=await response.json();
        console.log("RESPUESTA C#:", data);
        if(!response.ok){

            throw new Error(data.msj);

        }

        return data;

    }

    async register(
    userFullName:string,
    userName:string,
    userPasswordPlain:string,
    userEmail:string,
    userPhoneNumber:string,
    userCountryId:number,
    userGenderId:number,
    userBirthDay:string
):Promise<RegisterResponse>{

    const response=await fetch(`${API_URL}/api/Users/insertar`,{

        method:"POST",

        headers:{
            "Content-Type":"application/json"
        },

        body:JSON.stringify({

            userFullName,
            userName,
            userPasswordPlain,
            userEmail,
            userPhoneNumber,
            userCountryId,
            userGenderId,
            userBirthDay,

            //Estos los controla la API
            userCreatorId:1,
            userStatusId:1

        })

    });

    const data=await response.json();

    console.log("RESPUESTA REGISTER:",data);

    if(!response.ok){

        throw new Error(data.msj);

    }

    return data;

}

}


export const authRemoteDataSource=new AuthRemoteDataSource();