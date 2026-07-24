import { AuthRepository } from "../../Domain/repositories/AuthRepository";
import { User } from "../../Domain/entities/User";
import { localDataSource } from "../dataSources/LocalDataSource";
import { authRemoteDataSource } from "../dataSources/AuthRemoteDataSource";

export class AuthRepositoryImpl implements AuthRepository {

    async login(email: string, password: string): Promise<User> {

        const response =
            await authRemoteDataSource.login(email, password);

        return {

            id: response.data.userId.toString(),

            email: response.data.userEmail,

            name: response.data.userFullName,

            role: "user"

        };

    }

    async register(
        name: string,
        email: string,
        password: string,
        role?: "user" | "admin"
    ): Promise<User> {

        return localDataSource.register(
            name,
            email,
            password,
            role
        );

    }

    async getUsers(): Promise<User[]> {

        return localDataSource.getUsers();

    }

}