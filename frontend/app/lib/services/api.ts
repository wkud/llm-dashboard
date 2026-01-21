import { Prompt, CreatePromptDto } from '../types/prompt';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:8080';

class ApiService {
  private baseUrl: string;

  constructor() {
    this.baseUrl = `${API_BASE_URL}/api/Prompt`;
  }

  async getAllPrompts(): Promise<Prompt[]> {
    const response = await fetch(this.baseUrl, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch prompts: ${response.statusText}`);
    }

    return response.json();
  }

  async createPrompt(dto: CreatePromptDto): Promise<Prompt> {
    const response = await fetch(this.baseUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(dto),
    });

    if (!response.ok) {
      throw new Error(`Failed to create prompt: ${response.statusText}`);
    }

    return response.json();
  }

  async getPromptById(id: string): Promise<Prompt> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch prompt: ${response.statusText}`);
    }

    return response.json();
  }

  async deletePrompt(id: string): Promise<void> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: 'DELETE',
    });

    if (!response.ok) {
      throw new Error(`Failed to delete prompt: ${response.statusText}`);
    }
  }
}

export const apiService = new ApiService();
