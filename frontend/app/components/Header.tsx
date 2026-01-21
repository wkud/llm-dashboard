'use client';

import { Box, Typography, IconButton, Tooltip } from '@mui/material';
import { AutoAwesome, Refresh } from '@mui/icons-material';

interface HeaderProps {
  promptCount: number;
  onRefresh: () => void;
}

export default function Header({ promptCount, onRefresh }: HeaderProps) {
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'flex-start',
        mb: 4,
      }}
    >
      <Box>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
          <AutoAwesome 
            sx={{ 
              fontSize: 28, 
              color: '#3b82f6',
              animation: 'pulse 2s ease-in-out infinite',
              '@keyframes pulse': {
                '0%, 100%': {
                  opacity: 1,
                },
                '50%': {
                  opacity: 0.7,
                },
              },
            }} 
          />
          <Typography 
            variant="h4" 
            component="h1" 
            sx={{ 
              fontWeight: 600,
              letterSpacing: '-0.02em',
              background: 'linear-gradient(135deg, #f1f5f9 0%, #cbd5e1 100%)',
              WebkitBackgroundClip: 'text',
              WebkitTextFillColor: 'transparent',
              backgroundClip: 'text',
            }}
          >
            LLM Dashboard
          </Typography>
        </Box>
        <Typography 
          variant="body2" 
          sx={{ 
            color: 'text.secondary',
            fontWeight: 400,
          }}
        >
          {promptCount} prompt{promptCount !== 1 ? 's' : ''} • Auto-refreshing
        </Typography>
      </Box>
      <Tooltip title="Refresh">
        <IconButton
          onClick={onRefresh}
          sx={{
            color: 'text.secondary',
            transition: 'all 0.2s cubic-bezier(0.4, 0, 0.2, 1)',
            '&:hover': {
              backgroundColor: 'rgba(148, 163, 184, 0.1)',
              color: 'text.primary',
              transform: 'rotate(180deg)',
            },
          }}
        >
          <Refresh />
        </IconButton>
      </Tooltip>
    </Box>
  );
}
