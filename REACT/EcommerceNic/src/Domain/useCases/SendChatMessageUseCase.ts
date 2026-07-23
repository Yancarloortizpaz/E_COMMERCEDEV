import { ChatbotRepository } from "../repositories/ChatbotRepository";


export class SendChatMessageUseCase {


    constructor(
        private repository:ChatbotRepository
    ){}


    async execute(
        message:string
    ){

        return await this.repository.sendMessage(message);

    }

}