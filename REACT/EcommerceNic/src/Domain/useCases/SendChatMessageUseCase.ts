import { ChatbotRepository } from "../repositories/ChatbotRepository";


export class SendChatMessageUseCase {


    constructor(
        private repository:ChatbotRepository
    ){}


    async execute(
        message: string,
        conversacionId?: number
    ) {
        return await this.repository.sendMessage(message, conversacionId);
    }

}