'use client';

import { Box, Typography, Paper } from '@mui/material';
import { Inbox as InboxIcon } from '@mui/icons-material';
import { Prompt } from '../lib/types/prompt';
import PromptItem from './PromptItem';

interface PromptListProps {
  prompts: Prompt[];
  onDelete: (id: string) => Promise<void>;
}

export default function PromptList({ prompts, onDelete }: PromptListProps) {
  if (prompts.length === 0) {
    return (
      <Paper
        elevation={0}
        sx={{
          p: 6,
          textAlign: 'center',
          backgroundColor: 'rgba(30, 41, 59, 0.6)',
          backdropFilter: 'blur(10px)',
          WebkitBackdropFilter: 'blur(10px)',
          border: '1px solid rgba(148, 163, 184, 0.1)',
          animation: 'fadeIn 0.4s ease-in',
          '@keyframes fadeIn': {
            from: {
              opacity: 0,
            },
            to: {
              opacity: 1,
            },
          },
        }}
      >
        <InboxIcon sx={{ fontSize: 64, color: 'text.secondary', mb: 2 }} />
        <Typography variant="h6" component="h2" gutterBottom sx={{ fontWeight: 600 }}>
          No prompts yet
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Submit your first prompt to get started
        </Typography>
      </Paper>
    );
  }

  return (
    <Box>
      <Typography variant="h6" component="h2" gutterBottom sx={{ mb: 2 }}>
        Submitted Prompts
      </Typography>
      {prompts.map((prompt) => (
        <PromptItem key={prompt.id} prompt={prompt} onDelete={onDelete} />
      ))}
    </Box>
  );
}
