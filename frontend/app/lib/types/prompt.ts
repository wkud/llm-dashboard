export enum PromptStatus {
  None = 0,
  Pending = 1,
  Processing = 2,
  Completed = 3,
  Failed = -1,
}

export interface Prompt {
  id: string;
  text: string;
  status: PromptStatus;
  errorMessage?: string | null;
  outputText?: string | null;
  createdAt: string;
  updatedAt?: string | null;
}

export interface CreatePromptDto {
  text: string;
}
