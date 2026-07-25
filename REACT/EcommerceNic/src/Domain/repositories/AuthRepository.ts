import { User } from '../entities/User';
import { LoginResponse } from "../../Data/dataSources/AuthRemoteDataSource";

export interface AuthRepository {

login(email: string, password: string): Promise<LoginResponse>;

  register(
    userFullName: string,
    userName: string,
    userPasswordPlain: string,
    userEmail: string,
    userPhoneNumber: string,
    userCountryId: number,
    userGenderId: number,
    userBirthDay: string
  ): Promise<any>;

  getUsers(): Promise<User[]>;
}