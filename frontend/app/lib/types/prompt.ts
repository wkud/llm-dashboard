export enum PromptStatus {
  None = 'None',
  Pending = 'Pending',
  Processing = 'Processing',
  Completed = 'Completed',
  Failed = 'Failed',
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
