'use client';

import {
  Box,
  Typography,
  Paper,
  Chip,
  IconButton,
  Tooltip,
  Collapse,
} from '@mui/material';
import {
  Delete as DeleteIcon,
  ExpandMore as ExpandMoreIcon,
  ExpandLess as ExpandLessIcon,
} from '@mui/icons-material';
import { Prompt, PromptStatus } from '../lib/types/prompt';
import { useState } from 'react';

interface PromptItemProps {
  prompt: Prompt;
  onDelete: (id: string) => Promise<void>;
}

const getStatusColor = (status: PromptStatus): 'default' | 'primary' | 'success' | 'error' | 'warning' => {
  switch (status) {
    case PromptStatus.Pending:
      return 'default';
    case PromptStatus.Processing:
      return 'primary';
    case PromptStatus.Completed:
      return 'success';
    case PromptStatus.Failed:
      return 'error';
    default:
      return 'default';
  }
};

const getStatusLabel = (status: PromptStatus): string => {
  switch (status) {
    case PromptStatus.Pending:
      return 'Pending';
    case PromptStatus.Processing:
      return 'Processing';
    case PromptStatus.Completed:
      return 'Completed';
    case PromptStatus.Failed:
      return 'Failed';
    default:
      return 'Unknown';
  }
};

export default function PromptItem({ prompt, onDelete }: PromptItemProps) {
  const [expanded, setExpanded] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    if (isDeleting) return;
    setIsDeleting(true);
    try {
      await onDelete(prompt.id);
    } catch (error) {
      console.error('Failed to delete prompt:', error);
    } finally {
      setIsDeleting(false);
    }
  };

  const hasResult = prompt.status === PromptStatus.Completed && prompt.outputText;
  const hasError = prompt.status === PromptStatus.Failed && prompt.errorMessage;
  const showExpandButton = hasResult || hasError;

  return (
    <Paper
      elevation={0}
      sx={{
        p: 3,
        mb: 2,
        backgroundColor: 'background.paper',
        border: '1px solid',
        borderColor: 'divider',
      }}
    >
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
        <Box sx={{ flex: 1, mr: 2 }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 1 }}>
            <Chip
              label={getStatusLabel(prompt.status)}
              color={getStatusColor(prompt.status)}
              size="small"
            />
            {showExpandButton && (
              <IconButton
                size="small"
                onClick={() => setExpanded(!expanded)}
                sx={{ color: 'text.secondary' }}
              >
                {expanded ? <ExpandLessIcon /> : <ExpandMoreIcon />}
              </IconButton>
            )}
          </Box>
          <Typography
            variant="body1"
            sx={{
              color: 'text.primary',
              whiteSpace: 'pre-wrap',
              wordBreak: 'break-word',
            }}
          >
            {prompt.text}
          </Typography>
        </Box>
        <Tooltip title="Delete prompt">
          <IconButton
            onClick={handleDelete}
            disabled={isDeleting}
            sx={{
              color: 'text.secondary',
              '&:hover': {
                color: 'error.main',
                backgroundColor: 'action.hover',
              },
            }}
          >
            <DeleteIcon />
          </IconButton>
        </Tooltip>
      </Box>

      <Collapse in={expanded}>
        <Box
          sx={{
            mt: 2,
            p: 2,
            backgroundColor: 'background.default',
            borderRadius: 1,
            border: '1px solid',
            borderColor: 'divider',
          }}
        >
          {hasResult && (
            <Box>
              <Typography variant="subtitle2" color="success.main" gutterBottom>
                Result:
              </Typography>
              <Typography
                variant="body2"
                sx={{
                  color: 'text.primary',
                  whiteSpace: 'pre-wrap',
                  wordBreak: 'break-word',
                }}
              >
                {prompt.outputText}
              </Typography>
            </Box>
          )}
          {hasError && (
            <Box>
              <Typography variant="subtitle2" color="error.main" gutterBottom>
                Error:
              </Typography>
              <Typography
                variant="body2"
                sx={{
                  color: 'error.main',
                  whiteSpace: 'pre-wrap',
                  wordBreak: 'break-word',
                }}
              >
                {prompt.errorMessage}
              </Typography>
            </Box>
          )}
        </Box>
      </Collapse>
    </Paper>
  );
}
