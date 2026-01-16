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
        backgroundColor: 'background.paper',
        border: '1px solid',
        borderColor: 'divider',
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
            backgroundColor: 'background.default',
            '& fieldset': {
              borderColor: 'divider',
            },
            '&:hover fieldset': {
              borderColor: 'divider',
            },
            '&.Mui-focused fieldset': {
              borderColor: 'primary.main',
            },
          },
          '& .MuiInputBase-input': {
            color: 'text.primary',
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
          }}
        >
          Submit Prompt
        </Button>
      </Box>
    </Paper>
  );
}
