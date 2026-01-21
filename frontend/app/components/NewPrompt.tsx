'use client';

import { useState, KeyboardEvent } from 'react';
import {
  Box,
  Typography,
  TextField,
  Button,
  Paper,
} from '@mui/material';
import { Send as SendIcon } from '@mui/icons-material';

interface NewPromptProps {
  onSubmit: (text: string) => Promise<void>;
  isSubmitting: boolean;
}

export default function NewPrompt({ onSubmit, isSubmitting }: NewPromptProps) {
  const [promptText, setPromptText] = useState('');

  const handleSubmit = async () => {
    if (!promptText.trim() || isSubmitting) return;

    try {
      await onSubmit(promptText.trim());
      setPromptText('');
    } catch (error) {
      console.error('Failed to submit prompt:', error);
    }
  };

  const handleKeyDown = (e: KeyboardEvent<HTMLDivElement>) => {
    if (e.key === 'Enter' && (e.metaKey || e.ctrlKey)) {
      e.preventDefault();
      handleSubmit();
    }
  };

  return (
    <Paper
      elevation={0}
      sx={{
        p: 3,
        mb: 4,
        backgroundColor: 'rgba(30, 41, 59, 0.6)',
        backdropFilter: 'blur(10px)',
        WebkitBackdropFilter: 'blur(10px)',
        border: '1px solid rgba(148, 163, 184, 0.1)',
        transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
        '&:hover': {
          borderColor: 'rgba(148, 163, 184, 0.2)',
        },
      }}
    >
      <Typography variant="h6" component="h2" gutterBottom sx={{ mb: 2 }}>
        New Prompt
      </Typography>
      <TextField
        fullWidth
        multiline
        rows={6}
        placeholder="Enter your prompt here..."
        value={promptText}
        onChange={(e) => setPromptText(e.target.value)}
        onKeyDown={handleKeyDown}
        disabled={isSubmitting}
        sx={{
          mb: 2,
          '& .MuiOutlinedInput-root': {
            backgroundColor: 'rgba(15, 23, 42, 0.5)',
            backdropFilter: 'blur(8px)',
            WebkitBackdropFilter: 'blur(8px)',
            transition: 'all 0.2s ease',
            '& fieldset': {
              borderColor: 'rgba(148, 163, 184, 0.1)',
            },
            '&:hover fieldset': {
              borderColor: 'rgba(148, 163, 184, 0.2)',
            },
            '&.Mui-focused fieldset': {
              borderColor: '#3b82f6',
              borderWidth: '2px',
            },
          },
          '& .MuiInputBase-input': {
            color: 'text.primary',
            '&::placeholder': {
              color: 'text.secondary',
              opacity: 0.6,
            },
          },
        }}
      />
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="body2" color="text.secondary">
          Press ⌘ + Enter to submit
        </Typography>
        <Button
          variant="contained"
          onClick={handleSubmit}
          disabled={!promptText.trim() || isSubmitting}
          startIcon={<SendIcon />}
          sx={{
            textTransform: 'none',
            fontWeight: 500,
            borderRadius: 2,
            px: 3,
            py: 1,
            transition: 'all 0.2s cubic-bezier(0.4, 0, 0.2, 1)',
            '&:hover:not(:disabled)': {
              transform: 'translateY(-1px)',
              boxShadow: '0 4px 12px rgba(59, 130, 246, 0.3)',
            },
            '&:disabled': {
              opacity: 0.5,
            },
          }}
        >
          Submit Prompt
        </Button>
      </Box>
    </Paper>
  );
}
